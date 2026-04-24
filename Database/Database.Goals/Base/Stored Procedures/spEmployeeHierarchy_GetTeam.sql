CREATE PROCEDURE [Base].[spEmployeeHierarchy_GetTeam]
     @CompanyUUID NVARCHAR(200),
     @UsersUUIDLoggedIn NVARCHAR(200) = '',
     @UsersUUIDManager NVARCHAR(200) = NULL
AS
BEGIN
    SELECT 
        h.[UUID] AS [EmployeeHierarchyUUID],
        h.[EmployeeBusinessUnitUUID],
        h.[BusinessUnit] AS [EmployeeBusinessUnit],
        h.[EmployeeDepartmentUUID],
        h.[Department] AS [EmployeeDepartment],
        h.[EmployeeJobsUUID],
        h.[Position] AS [EmployeeJobs],
        h.[EmployeeLevel] AS [Level],
        h.[EmployeeJobsCriticalRoles] AS [CriticalRoles],
        h.[EmployeeJobDisciplines] AS [Disciplines],
        h.[UsersUUIDManager],
        h.[firstnameManager] + ISNULL(' ' + h.[lastnameManager], '') AS [UserManager],
        h.[CreateDate] AS [DateStarted],
        NULL AS [DateEnded],
        h.[isDeleted] AS [isArchive]
    FROM [Base].[dbo].[vwEmployeeHierarchy] h
    WHERE 
        h.[CompanyUUID] = @CompanyUUID
        AND h.[isDeleted] = 0
        AND (
            @UsersUUIDManager IS NULL 
            OR @UsersUUIDManager = '' 
            OR h.[UsersUUIDManager] = @UsersUUIDManager
        );
END