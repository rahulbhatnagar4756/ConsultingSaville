




	CREATE VIEW [base].[vwEmployees]
	AS
	SELECT c.[recordID] [Companiesid]
	, c.[UUID] [CompaniesUUID]
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
	, c.[URLAddress] + '/Images/Avatars/' + CONVERT(NVARCHAR, u.[recordid]) + 'medium.jpg' [Image]
	, DATEDIFF(YEAR, u.[DateOfBirth], GETDATE()) [Age]
	, u.[DateOfBirth]
	, u.[TandCsigned]
	, da.[OriginalDate] [YearsOfExperienceDate]
	, da.[Years] [YearsOfExperienceYear]
	, da.[Months] [YearsOfExperienceMonth]

	, um.[firstname] + ' ' + um.[lastname] [FullnameManager] 
	, um.[email] [emailManager] 
	, um.[IDNumber] [IDNumberManager]
	, um.[EmployeeNumber] [EmployeeNumberManager]
	
	, um.[isdeleted] [isDeletedUsersManager]

	FROM [dbo].[EmployeeJobs] j
	INNER JOIN [dbo].[EmployeeJobsDescription] jd ON jd.[EmployeeJobsid] = j.[Recordid]  
	INNER JOIN [dbo].[Companies] c ON c.[recordID] = jd.[Companyid]  
	LEFT OUTER JOIN [dbo].[EmployeeLevels] l ON l.[Id] = j.[EmployeeLevelsid]
	LEFT OUTER JOIN [dbo].[EmployeeJobsCriticalRoles] cr ON cr.[id] = j.[EmployeeJobsCriticalRolesid]
	LEFT OUTER JOIN [dbo].[EmployeeJobDisciplines] ejd ON ejd.[Id] = j.[EmployeeJobDisciplinesid] 

	LEFT OUTER JOIN [dbo].[EmployeeDepartments] d ON d.[Recordid] = jd.[EmployeeDepartmentsid] AND d.[isDeleted] = 0
	LEFT OUTER JOIN [dbo].[EmployeeBusinessUnits] bu ON bu.[Recordid] = d.[EmployeeBusinessUnitsid] AND bu.[isDeleted] = 0
	LEFT OUTER JOIN [dbo].[EmployeeBusinessUnitTypes] but ON but.[Id] = bu.[EmployeeBusinessUnitTypesid] AND but.[isDeleted] = 0

	INNER JOIN [dbo].[EmployeeHierarchy] h ON h.[EmployeeJobsid] = j.[Recordid] AND h.[isDeleted] = 0
	INNER JOIN [dbo].[Employees] e ON e.[Userid] = h.[Usersid] AND e.[isdeleted] = 0 AND e.[isActive] = 1
	INNER JOIN [dbo].[users] u ON u.[recordid] = h.[Usersid] AND u.[isdeleted] = 0 AND u.[isactive] = 1 
	INNER JOIN [dbo].[users] um ON um.[recordid] = h.[UsersidManager] AND um.[isdeleted] = 0  
	LEFT OUTER JOIN [dbo].[vwUserDataStore_Dates] da ON da.[Userid] = u.[recordid] AND da.[Typeid] = 80

	 
