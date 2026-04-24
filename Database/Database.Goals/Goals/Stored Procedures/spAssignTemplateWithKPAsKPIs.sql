USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spAssignTemplateWithKPAsKPIs1]    Script Date: 03-12-2025 17:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [Goals].[spAssignTemplateWithKPAsKPIs]
    @UsersUUID NVARCHAR(36),
    @TemplatesUUID NVARCHAR(36),
    @ContractPeriodsUUID NVARCHAR(36)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT,
            @TemplateId INT,
            @ContractPeriodId INT;

    -- 1️⃣ Get User ID
    SELECT @UserId = Id
    FROM [Base].[Users]
    WHERE UUID = @UsersUUID AND isDeleted = 0;

    IF @UserId IS NULL
    BEGIN
        RAISERROR('User not found', 16, 1);
        RETURN;
    END

    -- 2️⃣ Get Template ID
    SELECT @TemplateId = Id
    FROM [Goals].[Templates]
    WHERE UUID = @TemplatesUUID AND isDeleted = 0;

    IF @TemplateId IS NULL
    BEGIN
        RAISERROR('Template not found', 16, 1);
        RETURN;
    END

    -- 3️⃣ Get Contract Period ID
    SELECT @ContractPeriodId = Id
    FROM [Goals].[ContractPeriods]
    WHERE UUID = @ContractPeriodsUUID AND isDeleted = 0;

    IF @ContractPeriodId IS NULL
    BEGIN
        RAISERROR('Contract Period not found', 16, 1);
        RETURN;
    END

    -- 4️⃣ Check if template already assigned to this user for this contract period
    IF EXISTS (
        SELECT 1
        FROM [Goals].[UserTemplates]
        WHERE UsersId = @UserId 
          AND TemplatesId = @TemplateId 
          AND ContractPeriodsId = @ContractPeriodId
    )
    BEGIN
        RAISERROR('Template already assigned to this user for the selected contract period', 16, 1);
        RETURN;
    END

    -- Begin transaction
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 5️⃣ Assign Template to user for the contract period
        INSERT INTO [Goals].[UserTemplates] (TemplatesId, UsersId, ContractPeriodsId, DateCreated, isDeleted)
        VALUES (@TemplateId, @UserId, @ContractPeriodId, GETDATE(), 0);

        -- 6️⃣ Duplicate KPAs for this user
        DECLARE @KPA_Map TABLE (OldKPAId INT, NewKPAId INT);

        INSERT INTO [Goals].[KPA] (UUID, CompanyId, Usersid, Statusid, RatingPeriodsid, Name, Description, isActive, KPAidTemplate)
        OUTPUT INSERTED.KPAidTemplate, INSERTED.Id INTO @KPA_Map (OldKPAId, NewKPAId)
        SELECT NEWID(), CompanyId, @UserId, Statusid, RatingPeriodsid, Name + ' - Copy', Description, 1, KPAidTemplate
        FROM [Goals].[KPA]
        WHERE KPAidTemplate = @TemplateId AND isDeleted = 0;

        -- 7️⃣ Prepare KPI source data with mapping to new KPA
        DECLARE @KPI_Source TABLE (
            OldKPIId INT,
            Name NVARCHAR(200),
            Description NVARCHAR(MAX),
            CompanyId INT,
            StatusId INT,
            ToleranceSetsId INT,
            DateStart DATE,
            DateEnd DATE,
            Target FLOAT,
            IsScoreProcessing BIT,
            KPIidTemplate INT,
            NewKPAId INT
        );

        INSERT INTO @KPI_Source
        SELECT k.Id, k.Name, k.Description, k.CompanyId, k.StatusId, k.ToleranceSetsId,
               k.DateStart, k.DateEnd, k.Target, k.IsScoreProcessing, k.KPIidTemplate,
               km.NewKPAId
        FROM [Goals].[KPI] k
        INNER JOIN [Goals].[KPAKPI] kp ON k.Id = kp.KPIid
        INNER JOIN @KPA_Map km ON kp.KPAid = km.OldKPAId
        WHERE k.isDeleted = 0;

        -- 8️⃣ Duplicate KPIs and capture mapping
         DECLARE @KPI_Map TABLE (OldKPIId INT, NewKPIId INT, NewKPAId INT);

         INSERT INTO [Goals].[KPI] 
             (UUID, Companyid, Usersid, Statusid, ToleranceSetsid, Name, Description,
              DateStart, DateEnd, Target, isScoreProcessing, KPIidTemplate)
         --OUTPUT src.OldKPIId, INSERTED.Id, src.NewKPAId INTO @KPI_Map (OldKPIId, NewKPIId, NewKPAId)
         SELECT 
             NEWID(), 
             CompanyId, 
             @UserId, 
             StatusId, 
             ToleranceSetsId, 
             Name + ' - Copy', 
             Description,
             DateStart, 
             DateEnd, 
             Target, 
             IsScoreProcessing, 
             src.OldKPIId   -- <-- This sets KPIidTemplate to original KPI.Id
         FROM @KPI_Source AS src;
         
         -- 9️⃣ Insert KPA-KPI mapping with default weight
         INSERT INTO [Goals].[KPAKPI] (KPAid, KPIid, Weight)
         SELECT NewKPAId, NewKPIId, 100
         FROM @KPI_Map;


        COMMIT TRANSACTION;

        SELECT 1 AS isValid, 'Template assigned with KPAs and KPIs successfully.' AS Message;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 0 AS isValid, ERROR_MESSAGE() AS Message;
    END CATCH
END
