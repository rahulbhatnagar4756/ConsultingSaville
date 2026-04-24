
CREATE PROCEDURE [Goals].[spKPI_Duplicate]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @KPAUUID NVARCHAR(200),
    @KPIUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @CompanyId INT,
        @UsersidLoggedIn INT,
        @OldKPIId INT,
        @NewKPIId INT,
        @NewUUID NVARCHAR(200),
        @KPAId INT,
        @StatusId INT,
        @ToleranceSetsId INT,
        @Name NVARCHAR(500),
        @Description NVARCHAR(MAX),
        @DateStart DATETIME,
        @DateEnd DATETIME,
        @Target DECIMAL(18,4),
        @isScoreProcessing BIT,
        @TemplateId NVARCHAR(200),
        @Weights DECIMAL(18,4) = 100.0,
        @AuditLogsid BIGINT;

    -- Get Company ID
    SELECT @CompanyId = [recordID]
    FROM [Base].[dbo].[Companies]
    WHERE [UUID] = @CompanyUUID;

    IF ISNULL(@CompanyId,0) = 0
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'Valid company not supplied' AS [Message];
        RETURN 0;
    END

    -- Get Logged-in User ID
    SELECT TOP(1) @UsersidLoggedIn = [Id]
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    IF ISNULL(@UsersidLoggedIn,0) = 0
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'Valid logged-in user not supplied' AS [Message];
        RETURN 0;
    END

    -- Get KPA ID
    SELECT @KPAId = [Id]
    FROM [Goals].[KPA]
    WHERE [UUID] = @KPAUUID
      AND [Companyid] = @CompanyId
      AND [isDeleted] = 0;

    IF ISNULL(@KPAId,0) = 0
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'Valid KPA not found' AS [Message];
        RETURN 0;
    END

    -- Get existing KPI details with correct KPA
    SELECT TOP(1)
        @OldKPIId = k.[Id],
        @Name = k.[Name] + ' - Copy',
        @Description = k.[Description],
        @StatusId = k.[Statusid],
        @ToleranceSetsId = k.[ToleranceSetsid],
        @DateStart = k.[DateStart],
        @DateEnd = k.[DateEnd],
        @Target = k.[Target],
        @isScoreProcessing = k.[isScoreProcessing],
        @TemplateId = k.KPIidTemplate,
        @KPAId = kk.KPAid
    FROM [Goals].[KPI] k
    INNER JOIN [Goals].[KPAKPI] kk ON kk.KPIid = k.Id
    WHERE k.[UUID] = @KPIUUID
      AND k.[Companyid] = @CompanyId
      AND k.[isDeleted] = 0;

    IF @OldKPIId IS NULL
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'KPI not found' AS [Message];
        RETURN 0;
    END

    -- Generate new UUID for duplicated KPI
    SET @NewUUID = NEWID();

    -- Insert duplicated KPI
    INSERT INTO [Goals].[KPI] (
        [UUID],
        [Companyid],
        [Usersid],
        [Statusid],
        [ToleranceSetsid],
        [Name],
        [Description],
        [DateStart],
        [DateEnd],
        [Target],
        [isScoreProcessing],
        [KPIidTemplate]
    )
    VALUES (
        @NewUUID,
        @CompanyId,
        @UsersidLoggedIn,      -- or use original KPI user: k.Usersid
        @StatusId,
        @ToleranceSetsId,
        @Name,
        @Description,
        @DateStart,
        @DateEnd,
        @Target,
        @isScoreProcessing,
        @TemplateId
    );

    SET @NewKPIId = SCOPE_IDENTITY();

    -- Copy KPI-KPA relation
    EXEC [Goals].[spKPAKPI_Save] @NewKPIId, @KPAId, @Weights;

    -- Log creation
    EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, @UsersidLoggedIn, 'KPI', @NewKPIId, NULL, @AuditLogsid OUTPUT;

    -- Return result
    SELECT @NewUUID AS [UUID], 1 AS [isValid], 'KPI duplicated successfully' AS [Message];
END