USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spDeleteAssignedTemplateWithKPAsKPIs]    Script Date: 03-12-2025 17:08:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [Goals].[spDeleteAssignedTemplateWithKPAsKPIs]
    @UsersUUID NVARCHAR(200),
    @TemplatesUUID NVARCHAR(200),
    @ContractPeriodsUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT,
            @TemplateId INT,
            @ContractPeriodId INT;

    -- Validate User
    SELECT @UserId = Id
    FROM [Base].[Users]
    WHERE UUID = @UsersUUID AND isDeleted = 0;

    IF @UserId IS NULL
    BEGIN
        RAISERROR('User not found', 16, 1);
        RETURN;
    END

    -- Validate Template
    SELECT @TemplateId = Id
    FROM [Goals].[Templates]
    WHERE UUID = @TemplatesUUID AND isDeleted = 0;

    IF @TemplateId IS NULL
    BEGIN
        RAISERROR('Template not found', 16, 1);
        RETURN;
    END

    -- Validate Contract Period
    SELECT @ContractPeriodId = Id
    FROM [Goals].[ContractPeriods]
    WHERE UUID = @ContractPeriodsUUID AND isDeleted = 0;

    IF @ContractPeriodId IS NULL
    BEGIN
        RAISERROR('Contract Period not found', 16, 1);
        RETURN;
    END

    -- Ensure assignment exists
    IF NOT EXISTS (
        SELECT 1 FROM [Goals].[UserTemplates]
        WHERE UsersId = @UserId
          AND TemplatesId = @TemplateId
          AND ContractPeriodsId = @ContractPeriodId
    )
    BEGIN
        RAISERROR('Template is not assigned to this user for this contract period', 16, 1);
        RETURN;
    END

    BEGIN TRANSACTION;
    BEGIN TRY

        ------------------------------------------------------------
        -- 1️⃣ Identify cloned KPAs created for this template
        ------------------------------------------------------------
        DECLARE @KPA_Clones TABLE (KPAId INT);

        INSERT INTO @KPA_Clones
        SELECT Id
        FROM [Goals].[KPA]
        WHERE UsersId = @UserId
          AND KPAidTemplate = @TemplateId   -- cloned from template
          AND isDeleted = 0;

        ------------------------------------------------------------
        -- 2️⃣ Identify cloned KPIs created from original KPI template
        ------------------------------------------------------------
        DECLARE @KPI_Clones TABLE (KPIId INT);

        INSERT INTO @KPI_Clones
        SELECT Id
        FROM [Goals].[KPI]
        WHERE UsersId = @UserId
          AND KPIidTemplate IS NOT NULL      -- cloned KPIs
          AND isDeleted = 0;

        ------------------------------------------------------------
        -- 3️⃣ Delete KPA-KPI mappings
        ------------------------------------------------------------
        DELETE FROM [Goals].[KPAKPI]
        WHERE KPIid IN (SELECT KPIId FROM @KPI_Clones)
           OR KPAid IN (SELECT KPAId FROM @KPA_Clones);

        ------------------------------------------------------------
        -- 4️⃣ Delete cloned KPIs
        ------------------------------------------------------------
        DELETE FROM [Goals].[KPI]
        WHERE Id IN (SELECT KPIId FROM @KPI_Clones);

        ------------------------------------------------------------
        -- 5️⃣ Delete cloned KPAs
        ------------------------------------------------------------
        DELETE FROM [Goals].[KPA]
        WHERE Id IN (SELECT KPAId FROM @KPA_Clones);

        ------------------------------------------------------------
        -- 6️⃣ Remove UserTemplate assignment
        ------------------------------------------------------------
        DELETE FROM [Goals].[UserTemplates]
        WHERE UsersId = @UserId
          AND TemplatesId = @TemplateId
          AND ContractPeriodsId = @ContractPeriodId;

        COMMIT TRANSACTION;

        SELECT 1 AS isValid, 'Template assignment and all cloned KPAs/KPIs removed successfully.' AS Message;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 0 AS isValid, ERROR_MESSAGE() AS Message;
    END CATCH

END
