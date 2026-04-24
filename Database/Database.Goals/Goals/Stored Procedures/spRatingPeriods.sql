

CREATE PROCEDURE [Goals].[spRatingPeriods]
	  @CompanyUUID NVARCHAR(200)
	, @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN

	SELECT  
		  rp.[UUID]  
		, rp.[Name]
		, rp.[DisplayName]
	FROM [Goals].[vwRatingPeriods] rp
	WHERE rp.[CompanyUUID] = @CompanyUUID
		AND rp.[isDeleted] = 0

END