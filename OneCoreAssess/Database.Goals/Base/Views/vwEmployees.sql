






	CREATE VIEW [Base].[vwEmployees]
	AS
	SELECT ROW_NUMBER() OVER(PARTITION BY c.[recordID], h.[Usersid] ORDER BY h.[isDeleted] ASC, h.[CreateDate] DESC) [No]
	, c.[recordID] [Companyid]
	, c.[UUID] [CompanyUUID]
	, h.[Usersid]
	, u.[UUID] [usersUUID]
	, um.[recordid] [UsersidManager]
	, um.[UUID] [UsersUUIDManager]
	
	, j.[Recordid] [EmployeeJobsid]
	, j.[UUID] [EmployeeJobsUUID]
	
	, d.[Recordid] [EmployeeDepartmentsid] 
	, d.[UUID] [EmployeeDepartmentsUUID]
	
	, bu.[Recordid] [EmployeeBusinessUnitsid]
	, bu.[UUID] [EmployeeBusinessUnitsUUID]
	
	, but.[Id] [EmployeeBusinessUnitTypesid]
	, but.[UUID] [EmployeeBusinessUnitTypesUUID]

	   
	, but.[Name] [EmployeeBusinessUnitTypes]
	, bu.[Name] [EmployeeBusinessUnits]
	, d.[Name] [EmployeeDepartments]
	, j.[Name] [EmployeeJobs]
	, j.[EmployeeLevelsid]
	, l.[UUID] [EmployeeLevelsUUID]
	, l.[Name] [EmployeeLevels]
	, l.[LevelOrder]
	, j.[EmployeeJobsCriticalRolesid]
	, cr.[UUID] [EmployeeJobsCriticalRolesUUID]
	, cr.[Name] [EmployeeJobsCriticalRoles]
	, j.[EmployeeJobDisciplinesid]
	, ejd.[UUID] [EmployeeJobDisciplinesUUID]
	, ejd.[Name] [EmployeeJobDisciplines]

	, u.[firstname]
	, u.[MiddleName]
	, u.[lastname]
	, u.[email]
	, u.[mobile]
	, u.[IDNumber]
	, u.[EmployeeNumber]
	, u.[Race]
	, u.[Gender]
	, u.[isImage]
	, c.[URLAddress] + '/Images/Avatars/' + CONVERT(NVARCHAR, u.[Id]) + 'medium.jpg' [Image]
	, DATEDIFF(YEAR, u.[DateOfBirth], GETDATE()) [Age]
	, u.[DateOfBirth]
	, u.[TandCsigned]
	

	, um.[firstname] + ' ' + um.[lastname] [FullnameManager] 
	, um.[email] [emailManager] 
	, um.[IDNumber] [IDNumberManager]
	, um.[EmployeeNumber] [EmployeeNumberManager]
	
	, um.[isdeleted] [isDeletedUsersManager]

	FROM [Base].[dbo].[Employees] e
	INNER JOIN [Base].[users] u ON u.[Id] = e.[Userid] 
		AND u.[isdeleted] = 0 
		AND u.[isactive] = 1 
	INNER JOIN [Base].[dbo].[Companies] c ON c.[recordID] = e.[Companyid]  
		AND c.[isdeleted] = 0

	LEFT OUTER JOIN [Base].[EmployeeHierarchy] h ON h.[Usersid] = e.[Userid] 
		AND h.[isDeleted] = 0
		 
	LEFT OUTER JOIN [Base].[dbo].[users] um ON um.[recordid] = h.[UsersidManager] 
		AND um.[isdeleted] = 0 

	LEFT OUTER JOIN [Base].[dbo].[EmployeeJobs] j ON j.[Recordid] = h.[EmployeeJobsid]  
		AND j.[isdeleted] = 0
	LEFT OUTER JOIN [Base].[dbo].[EmployeeJobsDescription] jd ON jd.[EmployeeJobsid] = j.[Recordid]  
		

	LEFT OUTER JOIN [Base].[dbo].[EmployeeLevels] l ON l.[Id] = j.[EmployeeLevelsid]

	LEFT OUTER JOIN [Base].[dbo].[EmployeeJobsCriticalRoles] cr ON cr.[id] = j.[EmployeeJobsCriticalRolesid]
	LEFT OUTER JOIN [Base].[dbo].[EmployeeJobDisciplines] ejd ON ejd.[Id] = j.[EmployeeJobDisciplinesid] 

	LEFT OUTER JOIN [Base].[dbo].[EmployeeDepartments] d ON d.[Recordid] = jd.[EmployeeDepartmentsid] 
		AND d.[isDeleted] = 0
	LEFT OUTER JOIN [Base].[dbo].[EmployeeBusinessUnits] bu ON bu.[Recordid] = d.[EmployeeBusinessUnitsid] 
		AND bu.[isDeleted] = 0
	LEFT OUTER JOIN [Base].[dbo].[EmployeeBusinessUnitTypes] but ON but.[Id] = bu.[EmployeeBusinessUnitTypesid] 
		AND but.[isDeleted] = 0
		AND e.[Userid] = h.[Usersid] 



	WHERE e.[isdeleted] = 0 
		AND e.[isActive] = 1 
		 
