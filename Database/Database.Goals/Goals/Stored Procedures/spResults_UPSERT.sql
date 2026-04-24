
CREATE OR ALTER PROCEDURE [Goals].[spResults_UPSERT]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @Id INT = NULL,                   -- INT identity (NULL = INSERT)
    @KPI_UUID NVARCHAR(200),
    @RatingPeriod_UUID NVARCHAR(200),
    @ResultValue DECIMAL(18,2),
    @isActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @CompanyId INT,
        @UsersId INT,
        @ResultsIdForAudit INT,
        @KPIid INT,
        @RatingPeriodDatesid INT,
        @Score DECIMAL(18,4),
        @Target DECIMAL(18,4),
        @RangeStart DATETIME,
        @RangeEnd DATETIME,
        @MaxTotal DECIMAL(18,4),
        @KPITarget DECIMAL(18,4),
        @ToleranceSetsid INT,
        @ToleranceCount INT,
        @MatchedCount INT,
        @AuditLogsId BIGINT,
        @ResultUUID INT;   

    ------------------------------------------------------------
    -- Validate Company
    ------------------------------------------------------------
    SELECT @CompanyId = [recordID]
    FROM [Base].[dbo].[Companies]
    WHERE [UUID] = @CompanyUUID;

    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT CAST(NULL AS INT) AS [Id], 0 AS [isValid], 'Valid company not supplied' AS [Message];
        RETURN;
    END

    ------------------------------------------------------------
    -- Get User ID
    ------------------------------------------------------------
    SELECT TOP(1) @UsersId = [Id]
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    IF @UsersId IS NULL
    BEGIN
        SELECT CAST(NULL AS INT) AS [Id], 0 AS [isValid], 'Invalid user' AS [Message];
        RETURN;
    END

    ------------------------------------------------------------
    -- Convert KPI UUID → Numeric ID
    ------------------------------------------------------------
    SELECT @KPIid = [Id]
    FROM [Goals].[KPI]
    WHERE [UUID] = @KPI_UUID;

    IF ISNULL(@KPIid, 0) = 0
    BEGIN
        SELECT CAST(NULL AS INT) AS [Id], 0 AS [isValid], 'Invalid KPI UUID' AS [Message];
        RETURN;
    END

    ------------------------------------------------------------
    -- Convert RatingPeriod UUID → Numeric ID
    ------------------------------------------------------------
    SELECT @RatingPeriodDatesid = [Id]
    FROM [Goals].[RatingPeriods]
    WHERE [UUID] = @RatingPeriod_UUID;

    IF ISNULL(@RatingPeriodDatesid, 0) = 0
    BEGIN
        SELECT CAST(NULL AS INT) AS [Id], 0 AS [isValid], 'Invalid Rating Period UUID' AS [Message];
        RETURN;
    END

    ------------------------------------------------------------
    -- Get KPI Details
    ------------------------------------------------------------
    SELECT 
        @Target = kpi.Target,
        @MaxTotal = tl.MaxTotal,
        @RangeStart = kpi.DateStart,
        @RangeEnd = kpi.DateEnd,
        @KPITarget = kpi.Target,
        @ToleranceSetsid = kpi.ToleranceSetsid
    FROM [Goals].[KPI] AS kpi
    LEFT JOIN [Goals].[ToleranceSets] AS tl
        ON tl.Id = kpi.ToleranceSetsId
    WHERE kpi.Id = @KPIid;

    IF @KPITarget IS NULL
    BEGIN
        SELECT CAST(NULL AS INT) AS [Id], 0 AS [isValid], 'KPI not found' AS [Message];
        RETURN;
    END

    IF @ResultValue IS NULL
    BEGIN
        SELECT CAST(NULL AS INT) AS [Id], 0 AS [isValid], 'Result cannot be NULL' AS [Message];
        RETURN;
    END

    IF @KPITarget <= 0
    BEGIN
        SELECT CAST(NULL AS INT) AS [Id], 0 AS [isValid], 'KPI Target must be greater than zero' AS [Message];
        RETURN;
    END

    IF @MaxTotal IS NULL
        SET @MaxTotal = 100;

    ------------------------------------------------------------
    -- Count Tolerance Ranges
    ------------------------------------------------------------
    SELECT @ToleranceCount = COUNT(*)
    FROM [Goals].[ToleranceRanges]
    WHERE ToleranceSetsid = @ToleranceSetsid
      AND isDeleted = 0;

    ------------------------------------------------------------
    -- Apply Scoring Strategy
    ------------------------------------------------------------
    IF @ToleranceCount > 0
    BEGIN
        ;WITH ToleranceRangesWithEnd AS
        (
            SELECT 
                tr.Id,
                tr.ToleranceSetsid,
                tr.RangeStart,
                LEAD(tr.RangeStart) OVER (PARTITION BY tr.ToleranceSetsid ORDER BY tr.RangeStart) AS RangeEnd,
                tr.Score
            FROM [Goals].[ToleranceRanges] tr
            WHERE tr.ToleranceSetsid = @ToleranceSetsid
              AND tr.isDeleted = 0
        ),
        MatchedRanges AS
        (
            SELECT *,
                   ROW_NUMBER() OVER
                   (
                       ORDER BY
                         CASE 
                           WHEN RangeStart IS NOT NULL AND RangeEnd IS NOT NULL THEN 1
                           WHEN RangeStart IS NULL THEN 2
                           WHEN RangeEnd IS NULL THEN 3
                         END
                   ) AS Priority,
                   COUNT(*) OVER() AS TotalMatches
            FROM ToleranceRangesWithEnd
            WHERE 
                (RangeStart IS NOT NULL AND RangeEnd IS NOT NULL AND @ResultValue >= RangeStart AND @ResultValue < RangeEnd)
                OR (RangeStart IS NULL AND @ResultValue < RangeEnd)
                OR (RangeEnd IS NULL AND @ResultValue >= RangeStart)
        )
        SELECT 
            @Score = ROUND(Score, 4),
            @MatchedCount = TotalMatches
        FROM MatchedRanges
        WHERE Priority = 1;

        IF @MatchedCount IS NULL OR @MatchedCount <> 1
        BEGIN
            SELECT CAST(NULL AS INT) AS [Id], 0 AS [isValid], 'No or multiple tolerance ranges matched' AS [Message];
            RETURN;
        END
    END
    ELSE
    BEGIN
        SET @Score = ROUND((@ResultValue * 1.0 / @KPITarget) * 100, 4);
    END

    IF @Score < 0 SET @Score = 0;
    IF @Score > @MaxTotal SET @Score = @MaxTotal;


    -- Find existing ResultId for this KPI + RatingPeriod + Company + User
    SELECT @Id = Id
    FROM [Goals].[Results]
    WHERE KPIid = @KPIid
      AND RatingPeriodDatesid = @RatingPeriodDatesid
      AND isDeleted = 0;

    ------------------------------------------------------------
    -- INSERT
    ------------------------------------------------------------
    IF @Id IS NULL OR @Id = 0
    BEGIN
        INSERT INTO [Goals].[Results]
        (
            KPIid,
            RatingPeriodDatesid,
            Result,
            DateCreated,
            DateAdded,
            isActive,
            isDeleted,
            Score
        )
        VALUES
        (
            @KPIid,
            @RatingPeriodDatesid,
            @ResultValue,
            GETDATE(),
            GETDATE(),
            @isActive,
            0,
            @Score
        );

        SET @ResultsIdForAudit = SCOPE_IDENTITY();

        SELECT @ResultUUID = Id
        FROM [Goals].[Results]
        WHERE Id = @ResultsIdForAudit;

         EXEC [Goals].[spKPA_RecalculateScore] @KPIid,@RatingPeriodDatesid,@UsersUUIDLoggedIn, @CompanyId;

        EXEC [audit].[spAuditLogs_Log_Insert]
            @CompanyId, @UsersId, 'Results', @ResultsIdForAudit, NULL, @AuditLogsId OUTPUT;

        SELECT @ResultUUID AS [Id], 1 AS [isValid], 'Result created successfully' AS [Message];
        RETURN;
    END

    ------------------------------------------------------------
    -- UPDATE
    ------------------------------------------------------------
    IF NOT EXISTS (SELECT 1 FROM [Goals].[Results] WHERE [Id] = @Id AND isDeleted = 0)
    BEGIN
        SELECT CAST(NULL AS INT) AS [Id], 0 AS [isValid], 'Result not found or deleted' AS [Message];
        RETURN;
    END

    UPDATE [Goals].[Results]
    SET
        KPIid = @KPIid,
        RatingPeriodDatesid = @RatingPeriodDatesid,
        Result = @ResultValue,
        isActive = @isActive,
        Score = @Score
    WHERE Id = @Id;

    SET @ResultsIdForAudit = @Id;

    SELECT @ResultUUID = Id
    FROM [Goals].[Results]
    WHERE Id = @Id;

    EXEC [Goals].[spKPA_RecalculateScore] @KPIid,@RatingPeriodDatesid,@UsersUUIDLoggedIn, @CompanyId;

    EXEC [audit].[spAuditLogs_Log_Insert]
        @CompanyId, @UsersId, 'Results', @ResultsIdForAudit, NULL, @AuditLogsId OUTPUT;

    SELECT @ResultUUID AS [Id], 1 AS [isValid], 'Result updated successfully' AS [Message];
    RETURN;

END
