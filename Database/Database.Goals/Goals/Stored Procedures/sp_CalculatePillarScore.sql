
CREATE OR ALTER PROCEDURE [Goals].[sp_CalculatePillarScore]
    @CompanyId BIGINT,
    @KPAid BIGINT,
    @RatingPeriodDatesId INT,
    @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PillarId INT;
    DECLARE @PillarScore DECIMAL(18,4);
    DECLARE @UsersId BIGINT;

    --Get UserID based on KPAID 

    Select @UsersId=Usersid from [Goals].[KPA] where Id=@KPAid;

    -- Cursor to loop through all pillars linked to this KPA
    DECLARE PillarCursor CURSOR LOCAL FOR
    SELECT cp.Pillarsid
    FROM [Goals].[ContractPillarKPAs] cpk
    INNER JOIN [Goals].[ContractPillars] cp ON cp.Id = cpk.ContractPillarsid
    WHERE cpk.KPAid = @KPAid
      AND cpk.IsDeleted = 0
      AND cp.IsDeleted = 0;

    OPEN PillarCursor;
    FETCH NEXT FROM PillarCursor INTO @PillarId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- ===== Calculate Pillar Score =====
        SELECT @PillarScore =
            CASE WHEN SUM(c.Weight) = 0 THEN 0
                 ELSE SUM(r.Score * c.Weight) / SUM(c.Weight)
            END
        FROM [Goals].[ContractPillarKPAs] AS c
        INNER JOIN [Goals].[KPA] AS k
            ON k.Id = c.KPAid
            AND k.CompanyId = @CompanyId
            AND k.UsersId = @UsersId
            AND k.IsDeleted = 0
        INNER JOIN [Goals].[ResultsKPA] AS r
            ON r.KPAid = c.KPAid
            AND r.RatingPeriodDatesId = @RatingPeriodDatesId
            AND r.IsDeleted = 0
        WHERE c.ContractPillarsId IN (
            SELECT Id 
            FROM [Goals].[ContractPillars]
            WHERE PillarsId = @PillarId
              AND IsDeleted = 0
        )
        AND c.IsDeleted = 0;

        -- ===== Upsert into ResultsPillars =====
        IF EXISTS (
            SELECT 1
            FROM [Goals].[ResultsPillars]
            WHERE CompanyId = @CompanyId
              AND UsersId = @UsersId
              AND PillarsId = @PillarId
              AND RatingPeriodDatesId = @RatingPeriodDatesId
              AND IsDeleted = 0
        )
        BEGIN
            UPDATE [Goals].[ResultsPillars]
            SET Score = @PillarScore,
                DateAdded = GETUTCDATE()
            WHERE CompanyId = @CompanyId
              AND UsersId = @UsersId
              AND PillarsId = @PillarId
              AND RatingPeriodDatesId = @RatingPeriodDatesId
              AND IsDeleted = 0;
        END
        ELSE
        BEGIN
            INSERT INTO [Goals].[ResultsPillars]
            (
                CompanyId,
                UsersId,
                PillarsId,
                RatingPeriodDatesId,
                Score,
                DateCreated,
                DateAdded,
                IsDeleted
            )
            VALUES
            (
                @CompanyId,
                @UsersId,
                @PillarId,
                @RatingPeriodDatesId,
                @PillarScore,
                GETUTCDATE(),
                GETUTCDATE(),
                0
            );
        END

        FETCH NEXT FROM PillarCursor INTO @PillarId;
    END

    CLOSE PillarCursor;
    DEALLOCATE PillarCursor;

   EXEC [Goals].[sp_RecalculateFinalScore]  @CompanyId, @UsersId,  NULL , @UsersUUIDLoggedIn

END;
GO
