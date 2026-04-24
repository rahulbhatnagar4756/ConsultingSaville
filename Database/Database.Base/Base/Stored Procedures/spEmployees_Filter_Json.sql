 
CREATE PROCEDURE [base].[spEmployees_Filter_Json]
	  @CompanyUUID NVARCHAR(200) 
	, @UsersUUIDLoggedIn NVARCHAR(200) 
	, @json NVARCHAR(MAX) = NULL
	, @ProcessingYear int = NULL
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
	, @isCPP BIT = NULL
	, @isGoalFinalScore BIT = NULL
	, @isPersonality BIT = NULL
	, @isBox9 BIT = 0
	, @isFilterOnlySuccessionReadiness BIT = 0
	, @isFilterOnly9BoxTalentpool BIT = 0
	, @isFilterOnly9BoxPlotted BIT = 0
	, @isFilterOnly9BoxOutStanding BIT = 0
	, @FormSetsidPDP int = 16
	, @FormSetsidSuccessionPlanning int = 67   


	-------------------------------------------------------------------------------------------------------------------------------------
	-------------------------------------------------------------------------------------------------------------------------------------
	------------------------------------- get processing year from company settings------------------------------------------------------
	--SET @ProcessingYear = YEAR(DATEADD(year, -1, GETDATE()));

	SELECT @ProcessingYear = c.[ProcessingYear]
	FROM [dbo].[Company_Processing_Year] c
	WHERE c.[UUID] = @CompanyUUID; 
	
	 --public List<int>? Box9 { get; set; } 
	
	SELECT  @Search = ISNULL(MAX(CASE WHEN [Keys] = 'Search' THEN [Data] END), '')   
	, @isIncludeTeam = ISNULL(CASE WHEN ISNULL(MAX(CASE WHEN [Keys] = 'isIncludeTeam' THEN [Data] END), 'false') = 'true' THEN 1 ELSE 0 END, 0) 
	, @isGoalFinalScore = ISNULL(MAX(CASE WHEN [Keys] = 'isGoalFinalScore' THEN 
										CASE WHEN [Data] IS NULL THEN NULL ELSE 
											CASE WHEN [Data] = 'true' THEN 1 ELSE 0 END END END), 0) 
	, @isCPP = ISNULL(MAX(CASE WHEN [Keys] = 'isCPP' THEN 
							CASE WHEN [Data] IS NULL THEN NULL ELSE 
								CASE WHEN [Data] = 'true' THEN 1 ELSE 0 END END END), 0) 
	, @isPersonality = MAX(CASE WHEN [Keys] = 'isPersonality' THEN 
							CASE WHEN [Data] IS NULL THEN NULL ELSE 
								CASE WHEN [Data] = 'true' THEN 1 ELSE 0 END END END) 
	, @isFilterOnly9BoxTalentpool = ISNULL(MAX(CASE WHEN [Keys] = 'isFilterOnlyTalentpool' THEN 
											CASE WHEN [Data] IS NULL THEN NULL ELSE 
												CASE WHEN [Data] = 'true' THEN 1 ELSE 0 END END END), 0) 
	, @isFilterOnlySuccessionReadiness = ISNULL(MAX(CASE WHEN [Keys] = 'isFilterOnlySuccessionReadiness' THEN 
														CASE WHEN [Data] IS NULL THEN NULL ELSE 
															CASE WHEN [Data] = 'true' THEN 1 ELSE 0 END END END), 0) 
	, @isFilterOnly9BoxPlotted = ISNULL(MAX(CASE WHEN [Keys] = 'isFilterOnly9BoxPlotted' THEN 
												CASE WHEN [Data] IS NULL THEN NULL ELSE 
													CASE WHEN [Data] = 'true' THEN 1 ELSE 0 END END END),0) 
	, @isFilterOnly9BoxOutStanding = ISNULL(MAX(CASE WHEN [Keys] = 'isFilterOnlyOutStanding' THEN 
												CASE WHEN [Data] IS NULL THEN NULL ELSE 
													CASE WHEN [Data] = 'true' THEN 1 ELSE 0 END END END), 0) 
	FROM (SELECT [Key] [Keys], [Value] [Data]
		  FROM OPENJSON(@json)
		  WHERE [Key] IN ('Search'
						, 'isIncludeTeam'
						, 'isCPP'
						, 'isGoalFinalScore'
						, 'isPersonality'
						, 'isFilterOnlyTalentpool'
						, 'isFilterOnlySuccessionReadiness'
						, 'isFilterOnly9BoxPlotted'
						, 'isFilterOnlyOutStanding')) x;


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

	SELECT @isGender = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.Gender');

	SELECT @isEthnicity = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.Ethnicity');

	SELECT @isYearsOfExperience = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.YearsOfExperience');
	
	SELECT @isAge = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.Age');
	
	SELECT @isBox9 = CASE WHEN COUNT([value]) = 0 THEN 0 ELSE 1 END
	FROM OPENJSON(@json, '$.Box9');


	-------------------------------------------------------------------------------------------------------------------------------------
	-------------------------------------------------------------------------------------------------------------------------------------
	------------------------------------- Limit data acording to logged in users security privligious -----------------------------------

	-------------------------------------------------------------------------------------------------------------------------------------
	-------------------------------------------------------------------------------------------------------------------------------------



	SELECT e.[CompaniesUUID], 
    e.[UsersUUID], 
    e.[UsersUUIDManager], 
    e.[EmployeeJobsUUID], 
    e.[EmployeeDepartmentsUUID], 
    e.[EmployeeBusinessUnitsUUID], 
    e.[EmployeeBusinessUnitTypesUUID],
    e.[EmployeeLevelsUUID], 
    e.[EmployeeBusinessUnitTypes], 
    e.[EmployeeJobsCriticalRolesUUID], 
    e.[EmployeeJobDisciplinesUUID],
    e.[EmployeeBusinessUnits], 
    e.[EmployeeDepartments], 
    e.[EmployeeJobs], 
    e.[EmployeeLevels], 
	e.[LevelOrder],
    e.[EmployeeJobsCriticalRoles], 
    e.[EmployeeJobDisciplines],
    e.[FirstName], 
    e.[MiddleName], 
    e.[LastName], 
    e.[Email], 
    e.[Mobile], 
    e.[IDNumber], 
    e.[EmployeeNumber], 
    e.[Race], 
    e.[Gender], 
    e.[IsImage], 
    e.[Image], 
    e.[Age], 
	e.[DateOfBirth],
    e.[TandCsigned],
    e.[FullnameManager], 
    e.[EmailManager], 
    e.[IDNumberManager], 
    e.[EmployeeNumberManager],
    e.[IsDeletedUsersManager] 
	,  CASE WHEN ta.[recordId] IS NULL THEN 0 ELSE 1 END [Personality]
	,  CASE WHEN tacpp.[recordId] IS NULL THEN 0 ELSE 1 END [CPP]
	,  CASE WHEN p.[recordId] IS NULL THEN 0 ELSE 1 END [GoalFinalScore]
	, p.[Score] [FinalScore]
	, p.[ProcessDate] [Box9ProcessDate]
	, r2.[Box9Code] [Box9Name]
	, r2.[Box9id] [Box9Score]
	, (SELECT TOP(1) f.[UUID] 
			  FROM Forms f
			  WHERE f.[usersid] = e.[Usersid] AND f.[isDeleted] = 0 AND f.[FormSetsid] = @FormSetsidPDP
			  ORDER BY f.[Recordid] DESC) [FormsUUIDPDP]
	, (SELECT TOP(1) f.[UUID] 
		      FROM Forms f
		      WHERE f.[usersid] = e.[Usersid] AND f.[isDeleted] = 0 AND f.[FormSetsid] = @FormSetsidSuccessionPlanning 
		      ORDER BY f.[Recordid] DESC) [FormsUUIDSuccessionPlanning]
	
	FROM [base].[vwEmployees] e

	LEFT OUTER JOIN [dbo].[vwTestAnswersheets_Personality_Completed] ta ON ta.[userId] = e.[Usersid]  
	LEFT OUTER JOIN [dbo].[vwTestAnswersheets_CPP_Completed] tacpp ON tacpp.[userId] = e.[Usersid]  

	LEFT OUTER JOIN  [dbo].[vwGoalScore_Final_Score] p ON p.[Usersid] =  e.[Usersid] 
		AND YEAR(p.[ProcessDate]) = @ProcessingYear
	LEFT OUTER JOIN [pfm].[vwBox9Results] r2 ON r2.[UsersID] =  e.[Usersid] 
		AND r2.[Companyid] = e.[Companiesid]
		AND r2.[Year] = (@ProcessingYear -1)

	WHERE (@isBusinessUnitTypes = 0 OR e.[EmployeeBusinessUnitTypesUUID] IN (SELECT [value] 
																			 FROM OPENJSON(@json, '$.BusinessUnitTypes')))
	AND (@isBusinessUnits = 0 OR e.[EmployeeBusinessUnitsUUID] IN (SELECT [value] 
																   FROM OPENJSON(@json, '$.BusinessUnits')))
	AND (@isDepartment = 0 OR e.[EmployeeDepartmentsUUID] IN (SELECT [value] 
														      FROM OPENJSON(@json, '$.Departments')))
	AND (@isPosition = 0 OR e.[EmployeeJobsUUID] IN (SELECT [value] 
													 FROM OPENJSON(@json, '$.Position')))
	AND (@isLevel = 0 OR e.[EmployeeLevelsUUID] IN (SELECT [value] 
													FROM OPENJSON(@json, '$.Level')))
	AND (@isCriticalRoles = 0 OR e.[EmployeeJobsCriticalRolesUUID] IN (SELECT [value] 
																	   FROM OPENJSON(@json, '$.CriticalRoles')))
	AND (@isDisciplines = 0 OR e.[EmployeeJobDisciplinesUUID] IN (SELECT [value] 
																  FROM OPENJSON(@json, '$.Disciplines')))
	AND (@isGender = 0 OR e.[Gender] IN (SELECT [value] 
										 FROM OPENJSON(@json, '$.Gender')))
	AND (@isEthnicity = 0 OR e.[Race] IN (SELECT [value] 
										    FROM OPENJSON(@json, '$.Ethnicity')))
	AND (@isYearsOfExperience = 0 OR e.[YearsOfExperienceYear] IN (SELECT [value] 
																   FROM OPENJSON(@json, '$.YearsOfExperience')))

	AND (@isAge = 0 OR e.[Age] IN (SELECT [value] 
								   FROM OPENJSON(@json, '$.Age'))) 

	AND (@isBox9 = 0 OR r2.[Box9id] IN (SELECT [value] 
										FROM OPENJSON(@json, '$.Box9'))) 								

	AND CASE WHEN @isCPP IS NULL THEN -1 ELSE 
			CASE WHEN tacpp.[recordId] IS NULL THEN 0 ELSE 1 END END IN (-1, CASE WHEN tacpp.[recordId] IS NULL THEN 0 ELSE 1 END)

	AND CASE WHEN @isGoalFinalScore IS NULL THEN -1 ELSE 
			CASE WHEN p.[recordId] IS NULL THEN 0 ELSE 1 END END IN (-1, CASE WHEN p.[recordId] IS NULL THEN 0 ELSE 1 END)

	AND CASE WHEN @isPersonality IS NULL THEN -1 ELSE 
			CASE WHEN ta.[recordId] IS NULL THEN 0 ELSE 1 END END IN (-1, CASE WHEN ta.[recordId] IS NULL THEN 0 ELSE 1 END)


	AND e.[CompaniesUUID] = @CompanyUUID
	AND ISNULL(e.[firstname],'') + ' ' + ISNULL(e.[lastname], '') + ' ' + ISNULL(e.[firstname],'') + ' ' + ISNULL(e.[IDNumber],'') + ' ' + ISNULL(e.[EmployeeNumber],'') 
	+ ' ' + ISNULL(e.[email],'') + ' ' + ISNULL(e.[mobile], '') + ' ' +
	ISNULL(CASE WHEN @isIncludeTeam = 1 THEN e.[FullnameManager] END, '') + ' ' + 
	ISNULL(CASE WHEN @isIncludeTeam = 1 THEN e.[IDNumberManager] END, '') + ' ' +  
	ISNULL(CASE WHEN @isIncludeTeam = 1 THEN e.[EmployeeNumberManager] END, '') + ' ' + 
	ISNULL(CASE WHEN @isIncludeTeam = 1 THEN e.[emailManager] END, '') LIKE '%' + @Search + '%'

	AND (@isFilterOnly9BoxTalentpool = 0 
		 OR [Box9id] in (SELECT CASE WHEN @isFilterOnly9BoxTalentpool = 0 THEN 1 END [Id]
						 UNION SELECT CASE WHEN @isFilterOnly9BoxTalentpool = 0 THEN 2 END
						 UNION SELECT CASE WHEN @isFilterOnly9BoxTalentpool = 0 THEN 3 END
						 UNION SELECT CASE WHEN @isFilterOnly9BoxTalentpool = 0 THEN 4 END
						 UNION SELECT CASE WHEN @isFilterOnly9BoxTalentpool = 0 THEN 7 END
						 UNION SELECT 5 
						 UNION SELECT 6 
						 UNION SELECT 8 
						 UNION SELECT 9)) 

	AND (@isFilterOnly9BoxPlotted = 0 
		 OR r2.[Box9Resultsid] IS NOT NULL)

	AND (@isFilterOnly9BoxOutStanding = 0
		 OR r2.[Box9Resultsid] IS NULL)
	
	GROUP BY e.[CompaniesUUID], e.[UsersUUID], e.[UsersUUIDManager], e.[EmployeeJobsUUID], e.[EmployeeDepartmentsUUID], e.[EmployeeBusinessUnitsUUID], e.[EmployeeBusinessUnitTypesUUID],
	e.[EmployeeLevelsUUID], e.[EmployeeBusinessUnitTypes], e.[EmployeeJobsCriticalRolesUUID], e.[EmployeeJobDisciplinesUUID],e.[EmployeeBusinessUnits], e.[EmployeeDepartments], 
	e.[EmployeeJobs], e.[EmployeeLevels], e.[EmployeeJobsCriticalRoles], e.[EmployeeJobDisciplines],e.[FirstName], e.[MiddleName], e.[LastName], e.[Email], e.[Mobile], e.[IDNumber], 
	e.[EmployeeNumber], e.[Race], e.[Gender], e.[IsImage], e.[Image], e.[Age], e.[DateOfBirth],e.[TandCsigned],e.[FullnameManager], e.[EmailManager], e.[IDNumberManager], 
	e.[EmployeeNumberManager], e.[IsDeletedUsersManager]

	,  ta.[recordId], tacpp.[recordId],  p.[recordId], p.[Score], p.[ProcessDate], r2.[Box9Code], r2.[Box9id], e.[Usersid], e.[LevelOrder]

END
