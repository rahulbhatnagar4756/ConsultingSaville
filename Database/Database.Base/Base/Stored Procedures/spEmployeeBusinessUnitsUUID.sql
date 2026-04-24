 

CREATE PROCEDURE [base].[spEmployeeBusinessUnitsUUID]
	  @CompanyUUID NVARCHAR(200) 
	, @UsersUUIDLoggedIn NVARCHAR(200) = ''
 AS
 BEGIN

	SELECT b.[UUID]
	, t.[UUID] [TrackingUUID]
	, b.[Name]
	, b.[Description]
	FROM [dbo].[EmployeeBusinessUnits] b
	INNER JOIN [dbo].[EmployeeDepartments] d ON d.[EmployeeBusinessUnitsid] = b.[Recordid] AND d.[isDeleted] = 0
	INNER JOIN [dbo].[Companies] c ON c.[recordID] = d.[Companyid] AND c.[UUID] = @CompanyUUID
	LEFT OUTER JOIN [dbo].[EmployeeBusinessUnitTypes] t ON t.[Id] = b.[EmployeeBusinessUnitTypesid] AND t.[isDeleted] = 0
	WHERE b.[isDeleted] = 0 
	GROUP BY b.[UUID], t.[UUID], b.[Name], b.[Description]
 
END
