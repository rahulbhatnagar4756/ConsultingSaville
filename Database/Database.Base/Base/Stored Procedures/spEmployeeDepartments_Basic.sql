
CREATE PROCEDURE [base].[spEmployeeDepartments_Basic]
	@CompanyUUID NVARCHAR(200) 
	, @UsersUUIDLoggedIn NVARCHAR(200) 
AS
BEGIN

	SELECT d.[UUID]
	, d.[EmployeeBusinessUnitTypesUUID] 
	, d.[EmployeeBusinessUnitTypes] 
	, d.[EmployeeBusinessUnitsUUID]
	, d.[EmployeeBusinessUnits]
	, d.[EmployeeBusinessUnitsUUID] [ParentsUUID] 
	, d.[EmployeeBusinessUnits] [Parents] 
	, d.[Name]
	, d.[Description]
	, d.[Iconsid]
	, d.[IconColor]
	, d.[Icon]
	, ROW_NUMBER() OVER(ORDER BY d.[EmployeeBusinessUnitsid], d.[Name]) [OrderVal]
	, COUNT(d.[Id]) [Count]
	, 'Positions' [CountName]
	FROM [base].[vwEmployeeDepartments] d
	LEFT OUTER JOIN [base].[vwEmployeejobs] j ON j.[EmployeeDepartmentsid] = d.[Id] 
		AND d.[isDeleted] = 0
	WHERE d.[CompanyUUID] = @CompanyUUID
	AND d.[isDeleted] = 0
	GROUP BY d.[UUID]
	, d.[EmployeeBusinessUnitTypesUUID]  
	, d.[EmployeeBusinessUnitTypes]  
	, d.[EmployeeBusinessUnitsid]
	, d.[EmployeeBusinessUnitsUUID]
	, d.[EmployeeBusinessUnits]
	, d.[Name]
	, d.[Description]
	, d.[Iconsid]
	, d.[IconColor]
	, d.[Icon]

END
