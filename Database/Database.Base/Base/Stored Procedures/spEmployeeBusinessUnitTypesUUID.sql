--[base].[spEmployeeDepartmentsUUID]

CREATE PROCEDURE [base].[spEmployeeBusinessUnitTypesUUID]
	  @CompanyUUID NVARCHAR(200) 
	, @UsersUUIDLoggedIn NVARCHAR(200) = ''
 AS
 BEGIN

	SELECT b.[UUID]
	, NULL [TrackingUUID]
	, b.[Name]
	, NULL [Description]
	FROM [dbo].[EmployeeBusinessUnitTypes] b
	INNER JOIN [dbo].[Companies] c ON c.[recordID] = b.[Companyid] AND c.[UUID] = @CompanyUUID
	WHERE b.[isDeleted] = 0 
 
END
