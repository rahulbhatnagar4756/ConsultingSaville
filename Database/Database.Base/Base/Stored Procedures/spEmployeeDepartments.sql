

CREATE PROCEDURE [base].[spEmployeeDepartments]
	@Companyid int
AS
BEGIN
	SELECT d.[Recordid], d.[Companyid]
	, CASE WHEN bu.[Name] IS NULL THEN '' ELSE bu.[Name] + ' - ' END + d.[Name] [Name]
	, d.[Description]
	, d.[isDeleted]
	FROM [EmployeeDepartments] d
	LEFT OUTER JOIN [EmployeeBusinessUnits] bu ON bu.[Recordid] = d.[EmployeeBusinessUnitsid]
	WHERE d.[Companyid] = @Companyid AND d.[isDeleted] = 0 AND ISNULL(bu.[isDeleted], 0) = 0
	ORDER BY  CASE WHEN bu.[Name] IS NULL THEN '' ELSE bu.[Name] + ' - ' END + d.[Name]

END
