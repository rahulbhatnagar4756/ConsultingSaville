CREATE PROCEDURE [base].[spEmployeeHierarchy_By_UsersUUID]
	 @CompanyUUID nvarchar(200) 
	, @UsersUUIDLoggedIn nvarchar(200) = ''
	, @UsersUUID nvarchar(200) 
	, @isHistory bit = 0
AS
BEGIN

	SELECT h.[UUID] [EmployeeHierarchyUUID]
	, h.[EmployeeBusinessUnitUUID]
	, h.[BusinessUnit] [EmployeeBusinessUnit]
	, h.[EmployeeDepartmentUUID]
	, h.[Department] [EmployeeDepartment]

	, h.[EmployeeJobsUUID]
	, h.[Position] [EmployeeJobs]

	, h.[EmployeeLevel] [Level]
	, h.[EmployeeJobsCriticalRoles] [CriticalRoles]
	, h.[EmployeeJobDisciplines] [Disciplines]

	, h.[UsersUUIDManager]
	, h.[firstnameManager] + ISNULL(' ' + h.[lastnameManager],'') [UserManager]
	, h.[CreateDate] [DateStarted]
	, NULL [DateEnded]

	, h.[isDeleted] [isArchive]

	FROM [dbo].[vwEmployeeHierarchy] h


	WHERE h.[CompanyUUID] = @CompanyUUID AND h.[UsersUUID] = @UsersUUID
	AND (h.[isDeleted] = 0 OR h.[isDeleted] = @isHistory)

END
