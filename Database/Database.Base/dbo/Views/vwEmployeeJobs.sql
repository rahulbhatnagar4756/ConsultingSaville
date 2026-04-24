



 

CREATE VIEW [dbo].[vwEmployeeJobs]
AS
	SELECT ejd.[Companyid]
	, c.[UUID] [CompanyUUID]
    , but.[Id] [EmployeeBusinessUnitTypesid]
	, but.[UUID] [EmployeeBusinessUnitTypesUUID]
	, but.[Name] [EmployeeBusinessUnitTypes]

	, bu.[Recordid] [EmployeeBusinessUnitsid]
	, bu.[UUID] [EmployeeBusinessUnitsUUID]
	, bu.[Name] [EmployeeBusinessUnits]	
	
	, d.[Recordid] [EmployeeDepartmentsid]
	, d.[UUID] [EmployeeDepartmentsUUID]
	, d.[Name] [EmployeeDepartments] 

	, ej.[Recordid] [EmployeeJobsid]
	, ej.[UUID] [EmployeeJobsUUID]
	, ej.[EmployeeJobDisciplinesid]
	, jd.[UUID] [EmployeeJobDisciplinesUUID]
	, jd.[Name] [EmployeeJobDisciplines]
	, ej.[Name] [EmployeeJobs]
	, ej.[isDeleted]
	, ej.[Jobsid]
	, ej.[EmployeeJobsCriticalRolesid]
	, r.[UUID] [EmployeeJobsCriticalRolesUUID]
	, r.[Name] [EmployeeJobsCriticalRoles]
	, ej.[EmployeeLevelsid]
	, l.[UUID] [EmployeeLevelsUUID]
	, l.[Name] [Level]

	, ej.[Code]

	, ejd.[Description]
	, ejd.[Education]
	, ejd.[Experience]
	, ejd.[Competencies]
	, ejd.[ExpectedSkills]
	, ejd.[WorkingBoundaries] 
 

	, ej.[isDeleted] [isDeletedEmployeeJobs]
	, d.[isDeleted] [isDeletedEmployeeDepartments]
	, bu.[isDeleted] [isDeletedEmployeeBusinessUnits]

  FROM [dbo].[EmployeeJobs] ej
  INNER JOIN [dbo].[EmployeeJobsDescription] ejd ON ejd.[EmployeeJobsid] = ej.[Recordid] AND ejd.[isDeleted] = 0 --AND ejd.[Companyid] = 208
  INNER JOIN [dbo].[Companies] c ON c.[recordID] = ejd.[Companyid] 

  LEFT OUTER JOIN [dbo].[EmployeeDepartments] d ON d.[Recordid] = ejd.[EmployeeDepartmentsid] 
  LEFT OUTER JOIN [dbo].[EmployeeBusinessUnits] bu ON bu.[Recordid] = d.[EmployeeBusinessUnitsid]
  LEFT OUTER JOIN [dbo].[EmployeeBusinessUnitTypes] but ON but.[Id] = bu.[EmployeeBusinessUnitTypesid]

  LEFT OUTER JOIN [dbo].[EmployeeLevels] l ON l.[Id] = ej.[EmployeeLevelsid]
  LEFT OUTER JOIN [dbo].[EmployeeJobsCriticalRoles] r ON r.[id] = ej.[EmployeeJobsCriticalRolesid] 
  LEFT OUTER JOIN [dbo].[EmployeeJobDisciplines] jd ON jd.[Id] = ej.[EmployeeJobDisciplinesid]