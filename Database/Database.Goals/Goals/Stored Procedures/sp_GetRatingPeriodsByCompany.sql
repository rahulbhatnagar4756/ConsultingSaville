USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[sp_GetRatingPeriodsByCompany]    Script Date: 12-12-2025 17:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [Goals].[sp_GetRatingPeriodsByCompany]
    @CompanyUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT;

    -- Get CompanyId from UUID
    SELECT @CompanyId = c.[RecordId]
    FROM [Base].[dbo].[Companies] c
    WHERE c.[UUID] = @CompanyUUID;

    -- Validate CompanyId
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        RAISERROR('Valid company not supplied', 16, 1);
        RETURN;
    END

    -- Get Rating Periods
    SELECT 
        rp.UUID AS RatingPeriodUUID,
        'Q' + CAST(DATEPART(QUARTER, rPD.DateStart) AS VARCHAR(1)) 
            + ' ' + CAST(YEAR(rPD.DateStart) AS VARCHAR(4)) AS RatingPeriodName,
            CAST(YEAR(rPD.DateStart) AS VARCHAR(4))+' - '+rp.Name  AS RatingPeriodDisplayName
    FROM [Goals].[RatingPeriods] AS rp
    INNER JOIN [Goals].[RatingPeriodDates] AS rPD
        ON rPD.RatingPeriodsId = rp.Id
    WHERE rp.CompanyId = @CompanyId;
END;