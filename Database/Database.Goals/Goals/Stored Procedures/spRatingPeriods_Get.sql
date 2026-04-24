USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spRatingPeriods_Get]    Script Date: 18/09/2025 15:13:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Goals].[spRatingPeriods_Get]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @RatingPeriodsUUID NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        rp.[UUID],
        rp.[Companyid],
        rp.[Name],
        rp.[DisplayName],
        rp.[isDeleted]
    FROM [Goals].[RatingPeriods] rp
    INNER JOIN [Base].[dbo].[Companies] c 
        ON rp.[Companyid] = c.[recordID] 
       AND c.[UUID] = @CompanyUUID
    WHERE rp.[isDeleted] = 0
      AND (@RatingPeriodsUUID IS NULL OR rp.[UUID] = @RatingPeriodsUUID);
END
