
CREATE OR ALTER   PROCEDURE [Goals].[spKPA_RecalculateScore]
    @KPIid BIGINT,
    @RatingPeriodDatesid INT,
    @UsersUUIDLoggedIn NVARCHAR(200),
    @CompanyId BIGINT,
    @EnforceWeights BIT = 0  -- preserve original optional weight enforcement
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @KPAid BIGINT;
    DECLARE @KPAScore DECIMAL(18,4);
    DECLARE @TotalWeight DECIMAL(18,4);
    DECLARE @IsEmployee BIT;

    ------------------------------------------------------------
    -- Cursor: get all KPA linked to the KPI
    ------------------------------------------------------------
    DECLARE kpa_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT DISTINCT KPAid
        FROM [Goals].[KPAKPI]
        WHERE KPIid = @KPIid
          AND isDeleted = 0;

    OPEN kpa_cursor;
    FETCH NEXT FROM kpa_cursor INTO @KPAid;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        --------------------------------------------------------
        -- 1. Total Weight of the KPA
        --------------------------------------------------------
        SELECT @TotalWeight = SUM(Weight)
        FROM [Goals].[KPAKPI]
        WHERE KPAid = @KPAid
          AND isDeleted = 0;

        --------------------------------------------------------
        -- 2. Optional Weight Enforcement
        --------------------------------------------------------
        IF @EnforceWeights = 1 AND ISNULL(@TotalWeight,0) <> 100
        BEGIN
            SET @KPAScore = 0;
            GOTO UPSERT;
        END

        --------------------------------------------------------
        -- 3. Check if user is employee
        --------------------------------------------------------
        EXEC @IsEmployee = [Goals].[sp_IsUserEmployee] @UsersUUIDLoggedIn;

        --------------------------------------------------------
        -- 4. Calculate Weighted KPA Score
        --------------------------------------------------------
        SELECT @KPAScore = SUM(ISNULL(r.Score,0) * kk.Weight) / NULLIF(SUM(kk.Weight),0)
        FROM [Goals].[KPAKPI] kk
        LEFT JOIN [Goals].[Results] r
            ON r.KPIid = kk.KPIid
           AND r.RatingPeriodDatesid = @RatingPeriodDatesid
           AND r.isDeleted = 0
        WHERE kk.KPAid = @KPAid
          AND kk.isDeleted = 0;

        SET @KPAScore = ISNULL(@KPAScore,0);

        --------------------------------------------------------
        -- 5. UPSERT into ResultsKPA (no history)
        --------------------------------------------------------
        UPSERT:
        MERGE [Goals].[ResultsKPA] AS tgt
        USING (SELECT @KPAid AS KPAid, @RatingPeriodDatesid AS RatingPeriodDatesid) src
           ON tgt.KPAid = src.KPAid
          AND tgt.RatingPeriodDatesid = src.RatingPeriodDatesid
        WHEN MATCHED THEN
            UPDATE SET
                Score = @KPAScore,
                DateAdded = GETDATE(),
                isActive = 1,
                isDeleted = 0,
                IsEmployee = @IsEmployee
        WHEN NOT MATCHED THEN
            INSERT (KPAid, RatingPeriodDatesid, Score, DateCreated, DateAdded, isActive, isDeleted, IsEmployee)
            VALUES (@KPAid, @RatingPeriodDatesid, @KPAScore, GETDATE(), GETDATE(), 1, 0, @IsEmployee);

        --------------------------------------------------------
        -- 6. Recalculate Pillar Score
        --------------------------------------------------------
        EXEC [Goals].[sp_CalculatePillarScore]
            @CompanyId,
            @KPAid,
            @RatingPeriodDatesid,
            @UsersUUIDLoggedIn;

        --------------------------------------------------------
        FETCH NEXT FROM kpa_cursor INTO @KPAid;
    END

    CLOSE kpa_cursor;
    DEALLOCATE kpa_cursor;
END;
