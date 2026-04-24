
CREATE   PROCEDURE [Goals].[spAssignTemplateToUserAndDuplicateKPAsKPIs]
(
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),   -- User performing assignment
    @UsersUUIDAssignedTo NVARCHAR(200), -- User receiving template
    @TemplatesUUID NVARCHAR(200),
    @ContractPeriodsUUID NVARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @CompanyId INT,
        @UserIdLoggedIn INT,
        @UserIdAssignedTo INT,
        @TemplateId INT,
        @ContractPeriodId INT,
        @KPAUUID NVARCHAR(200),
        @NewKPAUUID NVARCHAR(200),
        @KPIUUID NVARCHAR(200),
        @AuditLogsid BIGINT;

    BEGIN TRY
        -- Start Transaction
        BEGIN TRANSACTION;

        -- 1️⃣ Validate Company
        SELECT @CompanyId = recordID
        FROM [Base].[Companies]
        WHERE UUID = @CompanyUUID AND isDeleted = 0;

        IF @CompanyId IS NULL
        BEGIN
            RAISERROR('Invalid Company.', 16, 1);
        END

        -- 2️⃣ Validate Logged-in User
        SELECT TOP(1) @UserIdLoggedIn = Id
        FROM [Base].[Users]
        WHERE UUID = @UsersUUIDLoggedIn AND isDeleted = 0;

        IF @UserIdLoggedIn IS NULL
        BEGIN
            RAISERROR('Invalid Logged-in User.', 16, 1);
        END

        -- 3️⃣ Validate Assigned User
        SELECT TOP(1) @UserIdAssignedTo = Id
        FROM [Base].[Users]
        WHERE UUID = @UsersUUIDAssignedTo AND isDeleted = 0;

        IF @UserIdAssignedTo IS NULL
        BEGIN
            RAISERROR('Invalid Assigned User.', 16, 1);
        END

        -- 4️⃣ Validate Template
        SELECT @TemplateId = Id
        FROM [Goals].[Templates]
        WHERE UUID = @TemplatesUUID AND isDeleted = 0;

        IF @TemplateId IS NULL
        BEGIN
            RAISERROR('Invalid Template.', 16, 1);
        END

        -- 5️⃣ Validate Contract Period
        SELECT @ContractPeriodId = Id
        FROM [Goals].[ContractPeriods]
        WHERE UUID = @ContractPeriodsUUID AND isDeleted = 0;

        IF @ContractPeriodId IS NULL
        BEGIN
            RAISERROR('Invalid Contract Period.', 16, 1);
        END

        -- 6️⃣ Check for duplicate assignment
        IF EXISTS (
            SELECT 1 
            FROM [Goals].[UserTemplates] 
            WHERE UsersId = @UserIdAssignedTo
              AND TemplatesId = @TemplateId
              AND ContractPeriodsId = @ContractPeriodId
        )
        BEGIN
            RAISERROR('Template already assigned to this user for the selected contract period.', 16, 1);
        END

        -- 7️⃣ Assign Template to User
        INSERT INTO [Goals].[UserTemplates] 
               ([TemplatesId], [UsersId], [ContractPeriodsId], [DateCreated], [isDeleted])
        VALUES
               (@TemplateId, @UserIdAssignedTo, @ContractPeriodId, GETDATE(), 0);

        -- 8️⃣ Duplicate KPAs of the Template
        DECLARE kpa_cursor CURSOR FOR
            SELECT UUID 
            FROM [Goals].[KPA]
            WHERE KPAidTemplate = @TemplateId
              AND isDeleted = 0;

        OPEN kpa_cursor;
        FETCH NEXT FROM kpa_cursor INTO @KPAUUID;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Duplicate KPA
            DECLARE @KPAId INT, @OldName NVARCHAR(500), @OldDescription NVARCHAR(MAX), 
                    @StatusId INT, @RatingPeriodsid INT, @KPAidTemplate INT,
                    @ContractPillarsId INT, @Weight DECIMAL(18,4), @isSuccessful BIT, @Message NVARCHAR(MAX);

            -- Get existing KPA details
            SELECT 
                @KPAId = Id,
                @OldName = [Name],
                @OldDescription = [Description],
                @StatusId = [Statusid],
                @RatingPeriodsid = [RatingPeriodsid],
                @KPAidTemplate = Id
            FROM [Goals].[KPA]
            WHERE UUID = @KPAUUID;

            -- Get Contract Pillar & Weight
            SELECT 
                @ContractPillarsId = CP.Id,
                @Weight = CK.Weight
            FROM [Goals].[ContractPillarKPAs] CK
            INNER JOIN [Goals].[ContractPillars] CP ON CK.ContractPillarsId = CP.Id
            WHERE CK.KPAId = @KPAId
              AND CK.isDeleted = 0;

            -- Insert new KPA
            SET @NewKPAUUID = NEWID();

            INSERT INTO [Goals].[KPA] 
            ([UUID],[CompanyId],[Usersid],[Statusid],[RatingPeriodsid],[Name],[Description],[isActive],[KPAidTemplate])
            VALUES 
            (@NewKPAUUID,@CompanyId,@UserIdAssignedTo,@StatusId,@RatingPeriodsid,@OldName + ' - Copy',@OldDescription,1,@KPAidTemplate);

            DECLARE @NewKPAId INT = SCOPE_IDENTITY();

            -- Log KPA creation
            EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, @UserIdAssignedTo, 'KPA', @NewKPAId, NULL, @AuditLogsid OUTPUT;

            -- Link Contract Pillar
            EXEC [Goals].[spContractPillarKPAs_Save] 
                @CompanyId, 
                @UserIdAssignedTo, 
                NULL, 
                @ContractPillarsId, 
                @NewKPAId, 
                @Weight, 
                @isSuccessful OUTPUT, 
                @Message OUTPUT;

            IF @isSuccessful = 0
            BEGIN
                RAISERROR('Error linking Contract Pillar to KPA.',16,1);
            END

            -- 9️⃣ Duplicate all KPIs under this KPA
            DECLARE kpi_cursor CURSOR FOR
                SELECT k.UUID
                FROM [Goals].[KPI] k
                INNER JOIN [Goals].[KPAKPI] kk ON kk.KPIid = k.Id
                WHERE kk.KPAid = @KPAId;

            OPEN kpi_cursor;
            FETCH NEXT FROM kpi_cursor INTO @KPIUUID;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                DECLARE @OldKPIId INT, @Name NVARCHAR(500), @Description NVARCHAR(MAX), @StatusIdKPI INT, 
                        @ToleranceSetsId INT, @DateStart DATETIME, @DateEnd DATETIME, @Target DECIMAL(18,4),
                        @isScoreProcessing BIT, @TemplateIdKPI NVARCHAR(200), @NewKPIUUID NVARCHAR(200), @NewKPIId INT;

                -- Get existing KPI details
                SELECT TOP(1)
                    @OldKPIId = k.Id,
                    @Name = k.Name + ' - Copy',
                    @Description = k.Description,
                    @StatusIdKPI = k.Statusid,
                    @ToleranceSetsId = k.ToleranceSetsid,
                    @DateStart = k.DateStart,
                    @DateEnd = k.DateEnd,
                    @Target = k.Target,
                    @isScoreProcessing = k.isScoreProcessing,
                    @TemplateIdKPI = k.KPIidTemplate
                FROM [Goals].[KPI] k
                INNER JOIN [Goals].[KPAKPI] kk ON kk.KPIid = k.Id
                WHERE k.UUID = @KPIUUID;

                -- Insert duplicated KPI
                SET @NewKPIUUID = NEWID();
                INSERT INTO [Goals].[KPI] (
                    [UUID],[Companyid],[Usersid],[Statusid],[ToleranceSetsid],[Name],
                    [Description],[DateStart],[DateEnd],[Target],[isScoreProcessing],[KPIidTemplate]
                )
                VALUES (
                    @NewKPIUUID,@CompanyId,@UserIdAssignedTo,@StatusIdKPI,@ToleranceSetsId,@Name,
                    @Description,@DateStart,@DateEnd,@Target,@isScoreProcessing,@TemplateIdKPI
                );

                SET @NewKPIId = SCOPE_IDENTITY();

                -- Link KPI to new KPA
                INSERT INTO [Goals].[KPAKPI] (KPAid, KPIid, Weight, isDeleted)
                VALUES (@NewKPAId, @NewKPIId, 100.0, 0);

                -- Log KPI creation
                EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, @UserIdAssignedTo, 'KPI', @NewKPIId, NULL, @AuditLogsid OUTPUT;

                FETCH NEXT FROM kpi_cursor INTO @KPIUUID;
            END

            CLOSE kpi_cursor;
            DEALLOCATE kpi_cursor;

            FETCH NEXT FROM kpa_cursor INTO @KPAUUID;
        END

        CLOSE kpa_cursor;
        DEALLOCATE kpa_cursor;

        -- Commit Transaction
        COMMIT TRANSACTION;

        -- Return Success
        SELECT 
            @TemplateId AS [TemplateId],
            @UserIdAssignedTo AS [UserId],
            @ContractPeriodId AS [ContractPeriodId],
            1 AS [isValid],
            'Template assigned and all KPAs/KPIs duplicated successfully.' AS [Message];

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 
            NULL AS [TemplateId],
            NULL AS [UserId],
            NULL AS [ContractPeriodId],
            0 AS [isValid],
            'Error: ' + ERROR_MESSAGE() AS [Message];
    END CATCH
END