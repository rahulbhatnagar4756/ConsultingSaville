


CREATE PROCEDURE [base].[spUsers_Names_Search]
	  @CompanyUUID NVARCHAR(200)
	, @UsersUUIDLoggedIn NVARCHAR(200)
	, @Search NVARCHAR(500)
	, @isIncludeTeam bit = 0
AS
BEGIN

	SELECT u.[UUID]
	, u.[FirstName] + ' ' + u.[lastname] [Name]
	FROM [dbo].[users] u
	INNER JOIN [dbo].[Employees] e ON e.[Userid] = u.[recordid] AND e.[isdeleted] = 0 AND e.[isActive] = 1 
	INNER JOIN [dbo].[Companies] c ON c.[recordID] = e.[Companyid] AND c.[UUID] = @CompanyUUID 
	LEFT OUTER JOIN [dbo].[EmployeeHierarchy] h ON h.[Usersid] = u.[recordid] AND h.[isDeleted] = 0
	LEFT OUTER JOIN [dbo].[users] um ON um.[recordid] = h.[UsersidManager]
	LEFT OUTER JOIN [dbo].[GoalPerformanceContractTemplates] t ON t.[Usersid] = u.[recordid] 
	WHERE ISNULL(u.[FirstName],'') 
	+ ' ' + ISNULL(u.[lastname],'') 
	+ ' ' + ISNULL(u.[email],'') 
	+ ' ' + ISNULL(u.[mobile],'') 
	+ ' ' + ISNULL(u.[IDNumber],'') 
	+ ' ' + ISNULL(u.[EmployeeNumber],'') 
	+ ' ' + CASE WHEN @isIncludeTeam = 0 THEN '' ELSE ISNULL(um.[FirstName],'') END
	+ ' ' + CASE WHEN @isIncludeTeam = 0 THEN '' ELSE ISNULL(um.[lastname],'') END
	+ ' ' + CASE WHEN @isIncludeTeam = 0 THEN '' ELSE ISNULL(um.[email],'') END
	+ ' ' + CASE WHEN @isIncludeTeam = 0 THEN '' ELSE ISNULL(um.[mobile],'') END
	+ ' ' + CASE WHEN @isIncludeTeam = 0 THEN '' ELSE ISNULL(um.[IDNumber],'') END
	+ ' ' + CASE WHEN @isIncludeTeam = 0 THEN '' ELSE ISNULL(um.[EmployeeNumber],'') END
	LIKE '%' + @Search + '%'
	AND t.[Recordid] IS NULL
	ORDER BY u.[FirstName] , u.[lastname]


END
