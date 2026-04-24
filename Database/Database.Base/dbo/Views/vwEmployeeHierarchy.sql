




CREATE VIEW [dbo].[vwEmployeeHierarchy]
AS
SELECT  h.[Recordid]
	  , h.[UUID]
	  , jd.[Companyid]	
	  , c.[UUID] [CompanyUUID]
      , h.[CreateDate]
	  , ISNULL(LEAD(h.[CreateDate]) OVER (PARTITION BY h.[Usersid] ORDER BY h.[CreateDate]), GETDATE())  AS EndDate
      , h.[EmployeeJobsid]
	  , j.[UUID] [EmployeeJobsUUID]
	  , j.[Name] [Position]
	  , j.[EmployeeLevelsid]
	  , l.[Name] [EmployeeLevel]
	  , j.[EmployeeJobsCriticalRolesid]
	  , r.[Name] [EmployeeJobsCriticalRoles]
	  , j.[EmployeeJobDisciplinesid]
	  , jdis.[Name] [EmployeeJobDisciplines]
      , h.[Usersid]
	  , u.[UUID] [UsersUUID]
	  , u.[firstname], u.[lastname], u.[IDNumber], u.[EmployeeNumber], u.[email]
      , h.[UsersidManager]
	  , um.[UUID] [UsersUUIDManager]
	  , um.[firstname] [firstnameManager], um.[lastname] [lastnameManager], um.[IDNumber] [IDNumberManager], u.[EmployeeNumber] [EmployeeNumberManager], um.[email] [emailManager]
	  , d.[UUID] [EmployeeDepartmentUUID]
	  , d.[Name] [Department]
	  , bu.[UUID] [EmployeeBusinessUnitUUID]
	  , bu.[Name] [BusinessUnit]
      , h.[isDeleted]
  FROM [dbo].[EmployeeHierarchy] h
  INNER JOIN [dbo].[users] u ON u.[recordid] = h.[Usersid] 
  INNER JOIN [dbo].[Users] um ON um.[recordid] = h.[UsersidManager] 
  INNER JOIN [dbo].[EmployeeJobs] j ON j.[Recordid] = h.[EmployeeJobsid] 
  INNER JOIN [dbo].[EmployeeJobsDescription] jd ON jd.[EmployeeJobsid] = h.[EmployeeJobsid] AND jd.[isDeleted] = 0
  INNER JOIN [dbo].[Companies] c ON c.[recordID] = jd.[Companyid] 
  LEFT OUTER JOIN [dbo].[EmployeeDepartments] d ON d.[Recordid] = jd.[EmployeeDepartmentsid]
  LEFT OUTER JOIN [dbo].[EmployeeBusinessUnits] bu ON bu.[Recordid] = d.[EmployeeBusinessUnitsid] 
  LEFT OUTER JOIN [dbo].[EmployeeLevels] l ON l.[Id] = j.[EmployeeLevelsid] 
  LEFT OUTER JOIN [dbo].[EmployeeJobsCriticalRoles] r ON r.[id] = j.[EmployeeJobsCriticalRolesid]
  LEFT OUTER JOIN [dbo].[EmployeeJobDisciplines] jdis ON jdis.[id] = j.[EmployeeJobDisciplinesid]


