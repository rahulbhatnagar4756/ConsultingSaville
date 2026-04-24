




CREATE VIEW [Goals].[vwTemplateBusinessUnitDepartmentPositions]
AS
SELECT t.[id] [Templatesid]
, t.[UUID]  [TemplatesUUID]
, t.[Name]  [Templates]
, t.[Code]  [TemplatesCode]
, tt.[UUID]  
, but.[UUID] [EmployeeBusinessUnitTypesUUID]
, but.[Name] [EmployeeBusinessUnitTypes]
, bu.[UUID] [EmployeeBusinessUnitsUUID]
, bu.[Name] [EmployeeBusinessUnits]
, d.[UUID]  [EmployeeDepartmentsUUID]
, d.[Name]  [EmployeeDepartments]
, j.[UUID]  [EmployeeJobsUUID]
, j.[Name]  [EmployeeJobs]
, l.[UUID]  [EmployeeLevelsUUID]
, l.[Name]  [EmployeeLevels]
, cr.[UUID] [EmployeeJobsCriticalRolesUUID]
, cr.[Name] [EmployeeJobsCriticalRoles]
, jd.[UUID] [EmployeeJobDisciplinesUUID]
, jd.[Name] [EmployeeJobDisciplines]

FROM [Goals].[TemplateBusinessUnitDepartmentPositions] tt
INNER JOIN [Goals].[Templates] t ON t.[Id] = tt.[Templatesid] AND t.[isDeleted] = 0
LEFT OUTER JOIN [Base].[dbo].[EmployeeBusinessUnits] bu ON bu.[Recordid] = tt.[EmployeeBusinessUnitsid]
LEFT OUTER JOIN [Base].[dbo].[EmployeeBusinessUnitTypes] but ON but.[Id] = bu.[EmployeeBusinessUnitTypesid]
LEFT OUTER JOIN [Base].[dbo].[EmployeeDepartments] d ON d.[Recordid] = tt.[EmployeeDepartmentsid]
LEFT OUTER JOIN [Base].[dbo].[EmployeeJobs] j ON j.[Recordid] = tt.[EmployeeJobsid]
LEFT OUTER JOIN [Base].[dbo].[EmployeeLevels] l ON l.[Id] = j.[EmployeeLevelsid]
LEFT OUTER JOIN [Base].[dbo].[EmployeeJobsCriticalRoles] cr ON cr.[id] = j.[EmployeeJobsCriticalRolesid]
LEFT OUTER JOIN [Base].[dbo].[EmployeeJobDisciplines] jd ON jd.[Id] = j.[EmployeeJobDisciplinesid] 


WHERE  tt.[isDeleted] = 0
GROUP BY tt.[UUID], bu.[UUID] , bu.[Name] , d.[UUID] , d.[Name] , j.[UUID] , j.[Name] , l.[UUID] , l.[Name] 
, t.[id], t.[UUID], t.[Name], t.[Code] 
, but.[UUID] 
, but.[Name] 
, cr.[UUID]  
, cr.[Name]  
, jd.[UUID]  
, jd.[Name]  
