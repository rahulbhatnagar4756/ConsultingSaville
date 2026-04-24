
CREATE PROCEDURE [base].[spEmployeeBusinessUnitTypes]
	@CompanyUUID NVARCHAR(200)
	, @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN

	SELECT but.[UUID]
	, but.[Name]
	, null [Description]
	, but.[Iconsid]
	, but.[IconColor]
	, but.[Icon]
	, COUNT(bu.[id]) [Count]
	FROM [base].[vwEmployeeBusinessUnitTypes] but
	LEFT OUTER JOIN [base].[vwEmployeeBusinessUnits] bu ON bu.[EmployeeBusinessUnitTypesid] = but.[Id] 
		AND bu.[isDeleted] = 0
	WHERE but.[CompanyUUID] = @CompanyUUID
		AND but.[isDeleted] = 0
	GROUP BY but.[UUID]
		, but.[Name]
		, but.[Iconsid]
		, but.[IconColor]
		, but.[Icon]

END
