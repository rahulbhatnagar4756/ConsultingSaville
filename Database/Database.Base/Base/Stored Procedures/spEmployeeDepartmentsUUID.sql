--[base].[spEmployeeDepartmentsUUID]

CREATE PROCEDURE [base].[spEmployeeDepartmentsUUID]
	  @CompanyUUID NVARCHAR(200) 
	, @UsersUUIDLoggedIn NVARCHAR(200) = 'B99CD3DE-1B13-4024-B737-18E4A5A0BE4E'
 AS
 BEGIN

	SELECT d.[UUID]
	, b.[UUID] [TrackingUUID]
	, d.[Name]
	, ISNULL(b.[Name] + ' - ','') + d.[Name] [FullName]
	, d.[Description]
	FROM [dbo].[EmployeeDepartments] d
	INNER JOIN [dbo].[Companies] c ON c.[recordID] = d.[Companyid] AND c.[UUID] = @CompanyUUID
	LEFT OUTER JOIN [dbo].[EmployeeBusinessUnits] b ON b.[Recordid] = d.EmployeeBusinessUnitsid AND b.[isDeleted] = 0
	WHERE d.[isDeleted] = 0 
 
END

 
