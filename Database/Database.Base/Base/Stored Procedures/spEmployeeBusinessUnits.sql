
CREATE PROCEDURE [base].[spEmployeeBusinessUnits]
	@CompanyUUID NVARCHAR(200)
	, @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN

	SELECT bu.[UUID]
	, bu.[Name]
	, NULL [Description]
	, bu.[EmployeeBusinessUnitTypesUUID] [ParentsUUID] 
	, bu.[EmployeeBusinessUnitTypes] [Parents] 
	, bu.[Iconsid]
	, bu.[IconColor]
	, bu.[Icon]
	, ROW_NUMBER() OVER(ORDER BY bu.[EmployeeBusinessUnitTypesid], bu.[Name]) [OrderVal]
	, COUNT(d.[Id]) [Count]
	, 'Departments' [CountName]
	FROM [base].[vwEmployeeBusinessUnits] bu
	LEFT OUTER JOIN [base].[vwEmployeeDepartments] d ON d.[EmployeeBusinessUnitsid] = bu.[Id] 
		AND d.[isDeleted] = 0
	WHERE bu.[CompanyUUID] = @CompanyUUID
	AND bu.[isDeleted] = 0
	GROUP BY bu.[UUID]
	, bu.[EmployeeBusinessUnitTypesid]
	, bu.[EmployeeBusinessUnitTypesUUID]
	, bu.[EmployeeBusinessUnitTypes]
	, bu.[Name]
	, bu.[Iconsid]
	, bu.[IconColor]
	, bu.[Icon]

END
