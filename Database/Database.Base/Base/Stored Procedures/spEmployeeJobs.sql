 

create PROCEDURE [base].[spEmployeeJobs]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ej.[EmployeeJobsUUID] [UUID]
    , ej.[EmployeeJobs] [Name]
    , ej.[Code]
    , ej.[Level]
    , ej.[Description]
    , ej.[EmployeeBusinessUnitTypesUUID]
    , ej.[EmployeeBusinessUnitTypes]
    , ej.[EmployeeBusinessUnitsUUID]
    , ej.[EmployeeBusinessUnits]
    , ej.[EmployeeDepartmentsUUID]
    , ej.[EmployeeDepartments]
    , ej.[EmployeeJobDisciplinesid]   [DisciplineId]
    , ej.[EmployeeJobDisciplinesUUID]
    , ej.[EmployeeJobDisciplines]
    , ej.[EmployeeJobsCriticalRolesUUID] 
    , ej.[EmployeeJobsCriticalRoles] 
    , ej.[EmployeeLevelsUUID] 
    , ej.[Level]
    , j.[UUID] [JobsUUID]
    , j.[JobName] [JobsName]
    , ROW_NUMBER() OVER(ORDER BY ej.[EmployeeJobs]) AS [OrderVal]
    FROM [base].[vwEmployeeJobs] ej
    LEFT OUTER JOIN [dbo].[Jobs] j ON j.[recordID] = ej.[Jobsid]
    WHERE  ej.[CompanyUUID] = @CompanyUUID
        AND ej.[isDeleted] = 0
        AND ej.[isDeletedEmployeeBusinessUnits] = 0
        AND ej.[isDeletedEmployeeDepartments] = 0
        AND ej.[isDeletedEmployeeJobs] = 0
    GROUP BY ej.[EmployeeJobsUUID] 
    , ej.[EmployeeJobs] 
    , ej.[Code]
    , ej.[Level]
    , ej.[Description]
    , ej.[EmployeeBusinessUnitTypesUUID]
    , ej.[EmployeeBusinessUnitTypes]
    , ej.[EmployeeBusinessUnitsUUID]
    , ej.[EmployeeBusinessUnits]
    , ej.[EmployeeDepartmentsUUID]
    , ej.[EmployeeDepartments]
    , ej.[EmployeeJobDisciplinesid]  
    , ej.[EmployeeJobDisciplinesUUID]
    , ej.[EmployeeJobDisciplines]
    , ej.[EmployeeJobsCriticalRolesUUID] 
    , ej.[EmployeeJobsCriticalRoles] 
    , ej.[EmployeeLevelsUUID] 
    , ej.[Level]
    , j.[UUID] 
    , j.[JobName]


END