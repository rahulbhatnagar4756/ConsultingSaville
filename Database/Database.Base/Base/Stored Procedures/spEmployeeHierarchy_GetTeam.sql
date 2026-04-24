CREATE PROCEDURE [Base].[spEmployeeHierarchy_GetTeam]
     @CompanyUUID nvarchar(200) 
    , @UsersUUIDLoggedIn nvarchar(200) = ''
    , @UsersUUIDManager nvarchar(200) 

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


    WHERE h.[CompanyUUID] = @CompanyUUID 
        AND h.[UsersUUIDManager] = @UsersUUIDManager
        AND h.[isDeleted] = 0

END