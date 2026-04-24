USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spRatingPeriodDates_Get]    Script Date: 18/09/2025 15:10:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Goals].[spRatingPeriodDates_Get]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @RatingPeriodDatesUUID NVARCHAR(200) = NULL,
    @RatingPeriodsUUID NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        rpd.[UUID],
        rpd.[RatingPeriodsid],
        rp.[UUID] AS [RatingPeriodsUUID],
        rp.[Name] AS [RatingPeriodName],
        rp.[DisplayName] AS [RatingPeriodDisplayName],
        rpd.[Name],
        rpd.[DateStart],
        rpd.[DateEnd],
        rpd.[DateOpen],
        rpd.[DateClose],
        rpd.[isActive],
        rpd.[isDeleted]
    FROM [Goals].[RatingPeriodDates] rpd
    INNER JOIN [Goals].[RatingPeriods] rp 
        ON rpd.[RatingPeriodsid] = rp.[Id]
    INNER JOIN [Base].[dbo].[Companies] c 
        ON rp.[Companyid] = c.[recordID] 
       AND c.[UUID] = @CompanyUUID
    WHERE rpd.[isDeleted] = 0
      AND rp.[isDeleted] = 0
      AND (@RatingPeriodDatesUUID IS NULL OR rpd.[UUID] = @RatingPeriodDatesUUID)
      AND (@RatingPeriodsUUID IS NULL OR rp.[UUID] = @RatingPeriodsUUID);
END
