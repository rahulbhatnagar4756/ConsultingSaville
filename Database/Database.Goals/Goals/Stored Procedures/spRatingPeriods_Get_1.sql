CREATE PROCEDURE [Goals].[spRatingPeriods_Get]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @RatingPeriodsUUID NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        rp.[UUID],
        rp.[CompanyUUID],
        rp.[Name],
        rp.[DisplayName]
    FROM [Goals].[vwRatingPeriods] rp
    WHERE rp.[isDeleted] = 0
      AND (ISNULL(@RatingPeriodsUUID, 0) = 0 
           OR rp.[UUID] = @RatingPeriodsUUID)
      AND rp.[CompanyUUID] = @CompanyUUID; 

END