USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spKPI_Save]    Script Date: 03/12/2025 16:57:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [Goals].[spKPI_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UsersUUID NVARCHAR(200),
    @KPIUUID NVARCHAR(200) = NULL,
    @KPAUUID NVARCHAR(200),
    @Name NVARCHAR(500),
    @Description NVARCHAR(MAX) = NULL,
    @StatusUUID NVARCHAR(200),
    --@RatingPeriodsUUID NVARCHAR(200),
    @ToleranceSetsUUID NVARCHAR(200) = NULL,
    @DateStart DATETIME = NULL,
    @DateEnd DATETIME = NULL,
    @Target DECIMAL(18,4) = 100.0,
    @Weights DECIMAL(18,4) = 100.0,
    @TrackingUUID NVARCHAR(200) = NULL,
    @isScoreProcessing BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersidLoggedIn INT
    , @Usersid INT
    , @KPIId INT
    , @KPAId INT
    , @ToleranceSetsId INT = NULL
    , @Statusid INT = 1 -- Default to Active status
    , @RatingPeriodid INT = NULL
    , @NewUUID NVARCHAR(200)
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 7
    , @AuditLogsid BIGINT
    , @isUpdate BIT = 0

    -- Get Company ID
    SELECT @CompanyId = [recordID] 
    FROM [Base].[dbo].[Companies] 
    WHERE [UUID] = @CompanyUUID;
    
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
        RETURN 0;
    END

    -- Get Logged-in User ID
    SELECT TOP(1) @UsersidLoggedIn = [Id]
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    IF ISNULL(@UsersidLoggedIn, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid logged-in user not supplied' [Message]
        RETURN 0;
    END

    -- Get Target User ID
    SELECT TOP(1) @Usersid = [Id]
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUID;

    IF ISNULL(@Usersid, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid target user not supplied' [Message]
        RETURN 0;
    END

    -- Get KPA ID (this links the KPI to a KPA)
    SELECT @KPAId = [Id]
    FROM [Goals].[KPA]
    WHERE [UUID] = @KPAUUID 
      AND [Companyid] = @CompanyId
      AND [isDeleted] = 0;

    IF ISNULL(@KPAId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid KPA not found' [Message]
        RETURN 0;
    END

    IF @StatusUUID IS NOT NULL
    BEGIN
        SELECT @StatusId = s.[Id]
        FROM [Goals].[Status] s
        WHERE s.[UUID] = @StatusUUID
            AND s.[isDeleted] = 0

        IF @StatusId IS NULL
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Valid status not found' [Message]
            RETURN 0;
        END

    END

    --IF @RatingPeriodsUUID IS NOT NULL
    --BEGIN
    --    SELECT @RatingPeriodid = s.[Id]
    --    FROM [Goals].[RatingPeriods] s
    --    WHERE s.[UUID] = @RatingPeriodsUUID
    --        AND s.[isDeleted] = 0

    --    IF @RatingPeriodid IS NULL
    --    BEGIN
    --        SELECT NULL [UUID], 0 [isValid], 'Valid rating period not found' [Message]
    --        RETURN 0;
    --    END
    --END

    -- Get ToleranceSets ID if provided
    IF @ToleranceSetsUUID IS NOT NULL AND @ToleranceSetsUUID != ''
    BEGIN
        SELECT @ToleranceSetsId = [Id]
        FROM [Goals].[ToleranceSets]
        WHERE [UUID] = @ToleranceSetsUUID 
          AND [Companyid] = @CompanyId
          AND [isDeleted] = 0;

        IF @ToleranceSetsId IS NULL
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Valid tolerance set not found' [Message]
            RETURN 0;
        END
    END
    ELSE
    BEGIN
        -- Set default tolerance set if none provided
        SET @ToleranceSetsId = 1; -- Assuming ID 1 is default
    END

        IF @Weights < 0 
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Weight must be larger than 0' [Message]
            RETURN 0;
        END

        -- Validate Target
        IF @Target is null
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Target must be provided' [Message]
            RETURN 0;
        END

    -- Check Access Rights
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

    IF @isHasAccess = 1
    BEGIN
        

        -- Check for duplicate Name within the same KPA (if Name is provided)
        --IF @Name IS NOT NULL AND @Name != ''
        --BEGIN
        --    DECLARE @ExistingKPIId INT = NULL;
            
        --    SELECT @ExistingKPIId = [Id]
        --    FROM [Goals].[KPI]
        --    WHERE [Name] = @Name 
        --      AND [KPAid] = @KPAId 
        --      AND [Companyid] = @CompanyId 
        --      AND [isDeleted] = 0
        --      AND (@KPIUUID IS NULL OR [UUID] != @KPIUUID);
            
        --    IF @ExistingKPIId IS NOT NULL
        --    BEGIN
        --        SELECT NULL [UUID], 0 [isValid], 'KPI name already exists for this KPA' [Message]
        --        RETURN 0;
        --    END
        --END
       

        IF ISNULL(@KPIUUID,'') = ''
        BEGIN
            -- Insert new record
            SET @NewUUID = NEWID();
        
            INSERT INTO [Goals].[KPI] (
                [UUID], 
                [Companyid], 
                [Usersid], 
                [Statusid],
                --[RatingPeriodsid],
                [ToleranceSetsid],
                [Name], 
                [Description],
                [DateStart],
                [DateEnd],
                [Target],
                [isScoreProcessing]
            )
            VALUES (
                @NewUUID, 
                @CompanyId, 
                @Usersid, 
                @StatusId,
                --@RatingPeriodid,
                @ToleranceSetsId,
                @Name, 
                @Description,
                @DateStart,
                @DateEnd,
                @Target, 
                @isScoreProcessing
            );
        
            SET @KPIId = SCOPE_IDENTITY();
                        
            EXEC [Goals].[spKPAKPI_Save] @KPIid, @KPAid, @Weights
 
            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                  @UsersidLoggedIn, 
                                                  'KPI', 
                                                  @KPIId, 
                                                  NULL, 
                                                  @AuditLogsid OUTPUT
        
            SELECT [UUID]
            , 1 [isValid]
            , 'KPI created successfully' as Message
            FROM [Goals].[KPI]
            WHERE [Id] = @KPIId; 
        END
        ELSE
        BEGIN
            -- Capture current values for comparison
            DECLARE @OldName NVARCHAR(500) = NULL
            , @OldDescription NVARCHAR(MAX) = NULL
            , @OldDateStart NVARCHAR(MAX) = NULL
            , @OldDateEnd NVARCHAR(MAX) = NULL
            , @OldTarget NVARCHAR(50) = NULL
            , @OldWeights NVARCHAR(50) = NULL
            , @OldTrackingUUID NVARCHAR(200) = NULL
            , @OldStatusid NVARCHAR(200) = NULL
            , @OldToleranceSetsid NVARCHAR(200) = NULL
            , @OldisScoreProcessing NVARCHAR(2) = NULL



            SELECT @KPIId = [Id]
            , @OldName = [Name]
            , @OldDescription = [Description]
            , @OldDateStart = CONVERT(NVARCHAR(50),[DateStart], 120)
            , @OldDateEnd = CONVERT(NVARCHAR(50),[DateEnd], 120)
            , @OldTarget = [Target] 
            , @OldStatusid = [Statusid]
            , @OldToleranceSetsid = [ToleranceSetsid]
            , @OldisScoreProcessing = [isScoreProcessing]
           
            , @isUpdate = CASE WHEN [Name] != @Name OR
                                    ISNULL([Description], '') != ISNULL(@Description, '') OR
                                    ISNULL([DateStart], '') != ISNULL(@DateStart, '') OR
                                    ISNULL([DateEnd], '') != ISNULL(@DateEnd, '') OR
                                    [Target] != @Target OR
                                    ISNULL([Statusid], '') != ISNULL(@Statusid, '') OR
                                    ISNULL([ToleranceSetsid], '') != ISNULL(@ToleranceSetsId, '') OR
                                    ISNULL([isScoreProcessing], '') != ISNULL(@isScoreProcessing, '') 

                               THEN 1 ELSE 0 END
            FROM [Goals].[KPI]
            WHERE [UUID] = @KPIUUID 
              AND [Companyid] = @CompanyId
              AND [isDeleted] = 0;

            IF @KPIId IS NULL
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'KPI not found or access denied' [Message]
                RETURN 0;
            END

            EXEC [Goals].[spKPAKPI_Save] @KPIid, @KPAid, @Weights

            IF @isUpdate = 1
            BEGIN
                -- Update the record
                UPDATE [Goals].[KPI]
                SET [Name] = @Name,
                    [Description] = @Description,
                    [Statusid] = @Statusid,
                    --[RatingPeriodsid] = @RatingPeriodid,
                    [DateStart] = @DateStart,
                    [DateEnd] = @DateEnd,
                    [Target] = @Target,
                    [ToleranceSetsid] = @ToleranceSetsId,
                    [isScoreProcessing] = @isScoreProcessing
                WHERE [UUID] = @KPIUUID 
                  AND [Companyid] = @CompanyId
                  AND [isDeleted] = 0;


                -- Log the changes
                EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                      @UsersidLoggedIn, 
                                                      'KPI', 
                                                      @KPIId, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT

                -- Save only the values that have changed
                IF @OldName != @Name
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Name', @OldName

                IF ISNULL(@OldDescription, '') != ISNULL(@Description, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Description', @OldDescription

                IF ISNULL(@OldDateStart, '') != ISNULL(@DateStart, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'DateStart', @OldDateStart

                IF ISNULL(@OldDateEnd, '') != ISNULL(@DateEnd, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'DateEnd', @OldDateEnd

                IF @OldTarget != @Target
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Target', @OldTarget
                                                                                   
                IF @OldWeights != @Weights                                         
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Weights', @OldWeights

                IF ISNULL(@OldTrackingUUID, '') != ISNULL(@TrackingUUID, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'TrackingUUID', @OldTrackingUUID

                IF @OldStatusid != @Statusid
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Statusid', @OldStatusid

                IF @OldToleranceSetsid != @ToleranceSetsid
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'ToleranceSetsid', @OldToleranceSetsid
            END
        
            SELECT @KPIUUID [UUID], 1 as [isValid], 'KPI updated successfully' [Message];
        END

        
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END