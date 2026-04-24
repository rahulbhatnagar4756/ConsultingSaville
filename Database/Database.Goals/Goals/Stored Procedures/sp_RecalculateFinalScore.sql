CREATE OR ALTER PROCEDURE [Goals].[sp_RecalculateFinalScore]
(
    @CompanyId BIGINT = NULL,
    @UsersId BIGINT = NULL,
    @ScoringPeriodId INT = NULL,
    @UsersUUIDLoggedIn NVARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsEmployee BIT;

    EXEC @IsEmployee = [Goals].[sp_IsUserEmployee] @UsersUUIDLoggedIn;

    ;WITH ScoringPeriodWindow AS
    (
        SELECT
            SP.Id AS ScoringPeriodsid,
            SP.Companyid,
            RPD.Id AS RatingPeriodDatesid,
            -- Calculate scoring period start
            DATEFROMPARTS(
                YEAR(RPD.DateStart),
                SP.StartMonth,
                SP.StartDay
            ) AS PeriodStart,
            -- Calculate scoring period end (handle cross-year)
            CASE
                WHEN SP.EndMonth < SP.StartMonth
                    THEN DATEFROMPARTS(YEAR(RPD.DateStart) + 1, SP.EndMonth, SP.EndDay)
                ELSE DATEFROMPARTS(YEAR(RPD.DateStart), SP.EndMonth, SP.EndDay)
            END AS PeriodEnd
        FROM Goals.ScoringPeriods SP
        INNER JOIN Goals.RatingPeriodDates RPD
            ON SP.Companyid = SP.Companyid -- anchor for year context
        WHERE
            SP.isActive = 1
            AND SP.isDeleted = 0
            AND (@CompanyId IS NULL OR SP.Companyid = @CompanyId)
            AND (@ScoringPeriodId IS NULL OR SP.Id = @ScoringPeriodId)
    ),
    AggregatedScores AS
    (
        SELECT
            K.Companyid,
            K.Usersid,
            SPW.ScoringPeriodsid,
            AVG(RK.Score * 1.0) AS FinalScore
        FROM Goals.ResultsKPA RK
        INNER JOIN Goals.KPA K
            ON K.Id = RK.KPAid
        INNER JOIN Goals.RatingPeriodDates RPD
            ON RPD.Id = RK.RatingPeriodDatesid
        INNER JOIN ScoringPeriodWindow SPW
            ON SPW.Companyid = K.Companyid
           AND SPW.RatingPeriodDatesid = RPD.Id
           AND RPD.DateStart >= SPW.PeriodStart
           AND RPD.DateEnd   <= SPW.PeriodEnd
        WHERE
            RK.isActive = 1 AND RK.isDeleted = 0
            AND K.isActive = 1 AND K.isDeleted = 0
            AND (@UsersId IS NULL OR K.Usersid = @UsersId)
        GROUP BY
            K.Companyid,
            K.Usersid,
            SPW.ScoringPeriodsid
    )
    MERGE Goals.Score AS Target
    USING AggregatedScores AS Source
        ON  Target.Companyid = Source.Companyid
        AND Target.Usersid = Source.Usersid
        AND Target.ScoringPeriodsid = Source.ScoringPeriodsid
    WHEN MATCHED THEN
        UPDATE SET
            Target.Score = Source.FinalScore,
            Target.DateProcessed = GETUTCDATE(),
            Target.IsEmployee = @IsEmployee
    WHEN NOT MATCHED THEN
        INSERT
        (
            Companyid,
            Usersid,
            ScoringPeriodsid,
            Score,
            DateProcessed,
            IsEmployee
        )
        VALUES
        (
            Source.Companyid,
            Source.Usersid,
            Source.ScoringPeriodsid,
            Source.FinalScore,
            GETUTCDATE(),
            @IsEmployee
        );
END;
GO
