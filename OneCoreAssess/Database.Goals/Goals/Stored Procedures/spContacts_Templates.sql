 


CREATE PROCEDURE [Goals].[spContacts_Templates]
	@CompanyUUID NVARCHAR(200) 
	, @UsersUUIDLoggedIn NVARCHAR(200) 
	, @json NVARCHAR(MAX) = NULL 
AS
BEGIN

	IF LEN(@json) = 0
		SET @json = '{}'

	DECLARE @Search NVARCHAR(350) = ''
	, @isIncludeTeam BIT = 0
	, @isDepartment BIT = 0
	, @isBusinessUnitTypes BIT = 0
	, @isBusinessUnits BIT = 0
	, @isPosition BIT = 0
	, @isLevel BIT = 0
	, @isCriticalRoles BIT = 0
	, @isDisciplines BIT = 0
	, @isGender BIT = 0
	, @isEthnicity BIT = 0
	, @isYearsOfExperience BIT = 0
	, @isAge BIT = 0
	 

	 --public List<int>? Box9 { get; set; } 
	
	SELECT  @Search = ISNULL(MAX(CASE WHEN [Keys] = 'Search' THEN [Data] END), '')   
	, @isIncludeTeam = ISNULL(CASE WHEN ISNULL(MAX(CASE WHEN [Keys] = 'isIncludeTeam' THEN [Data] END), 'false') = 'true' THEN 1 ELSE 0 END, 0) 
	FROM (SELECT [Key] [Keys], [Value] [Data]
		  FROM OPENJSON(@json)
		  WHERE [Key] IN ('Search'
						, 'isIncludeTeam')) x;


	SELECT @isBusinessUnitTypes = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.BusinessUnitTypes');
	
	SELECT @isBusinessUnits = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.BusinessUnits');

	SELECT @isDepartment = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.Departments');

	SELECT @isPosition = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.Position');

	SELECT @isLevel = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.Level');

	SELECT @isCriticalRoles = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.CriticalRoles');

	SELECT @isDisciplines = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.Disciplines');
 


	-------------------------------------------------------------------------------------------------------------------------------------
	-------------------------------------------------------------------------------------------------------------------------------------
	------------------------------------- Limit data acording to logged in users security privligious -----------------------------------

	-------------------------------------------------------------------------------------------------------------------------------------
	-------------------------------------------------------------------------------------------------------------------------------------



	SELECT t.[CompanyUUID] 
    , t.[UUID] [TemplatesUUID]
	, t.[Name] [Templates] 
	, t.[Code]
    , tb.[EmployeeJobsUUID] 
    , tb.[EmployeeDepartmentsUUID] 
    , tb.[EmployeeBusinessUnitsUUID] 
    , tb.[EmployeeBusinessUnitTypesUUID]
    , tb.[EmployeeLevelsUUID] 
    , tb.[EmployeeBusinessUnitTypes] 
    , tb.[EmployeeJobsCriticalRolesUUID] 
    , tb.[EmployeeJobDisciplinesUUID]
    , tb.[EmployeeBusinessUnits] 
    , tb.[EmployeeDepartments] 
    , tb.[EmployeeJobs] 
    , tb.[EmployeeLevels]  
    , tb.[EmployeeJobsCriticalRoles] 
    , tb.[EmployeeJobDisciplines]
	, CONVERT(BIT, CASE WHEN c.[Id] IS NULL THEN 0 ELSE 1 END) [isValidContract]

	FROM [Goals].[vwTemplates] t
	INNER JOIN [Goals].[vwTemplateBusinessUnitDepartmentPositions] tb ON tb.[Templatesid] = t.[Id]


	LEFT OUTER JOIN [Goals].[vwContracts_Templates] c ON c.[Templatesid] = t.[Id]
		AND c.[isActive] = 1
		AND c.[isDeleted] = 0
 
	


	WHERE   (@isBusinessUnitTypes = 0 OR tb.[EmployeeBusinessUnitTypesUUID] IN (SELECT [value] 
																		   FROM OPENJSON(@json, '$.BusinessUnitTypes')))
	AND (@isBusinessUnits = 0 OR tb.[EmployeeBusinessUnitsUUID] IN (SELECT [value] 
																   FROM OPENJSON(@json, '$.BusinessUnits')))
	AND (@isDepartment = 0 OR tb.[EmployeeDepartmentsUUID] IN (SELECT [value] 
														      FROM OPENJSON(@json, '$.Departments')))
	AND (@isPosition = 0 OR tb.[EmployeeJobsUUID] IN (SELECT [value] 
													 FROM OPENJSON(@json, '$.Position')))
	AND (@isLevel = 0 OR tb.[EmployeeLevelsUUID] IN (SELECT [value] 
													FROM OPENJSON(@json, '$.Level')))
	AND (@isCriticalRoles = 0 OR tb.[EmployeeJobsCriticalRolesUUID] IN (SELECT [value] 
																	   FROM OPENJSON(@json, '$.CriticalRoles')))
	AND (@isDisciplines = 0 OR tb.[EmployeeJobDisciplinesUUID] IN (SELECT [value] 
																  FROM OPENJSON(@json, '$.Disciplines')))
 
	 

	 
	AND t.[CompanyUUID] = @CompanyUUID
	AND ISNULL(t.[Name],'') LIKE  '%' + @Search + '%'

	
	GROUP BY  t.[CompanyUUID] 
    , t.[UUID]  
	, t.[Name]
	, t.[Code]
    , tb.[EmployeeJobsUUID] 
    , tb.[EmployeeDepartmentsUUID] 
    , tb.[EmployeeBusinessUnitsUUID] 
    , tb.[EmployeeBusinessUnitTypesUUID]
    , tb.[EmployeeLevelsUUID] 
    , tb.[EmployeeBusinessUnitTypes] 
    , tb.[EmployeeJobsCriticalRolesUUID] 
    , tb.[EmployeeJobDisciplinesUUID]
    , tb.[EmployeeBusinessUnits] 
    , tb.[EmployeeDepartments] 
    , tb.[EmployeeJobs] 
    , tb.[EmployeeLevels]  
    , tb.[EmployeeJobsCriticalRoles] 
    , tb.[EmployeeJobDisciplines]
	, c.[Id]
	ORDER BY t.[Name]

END
