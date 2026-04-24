


	create VIEW [base].[vwUserscompanys]
	as
   SELECT uc.[Id]
      , uc.[Companyid]
	  , c.[UUID] [CompanyUUID]
	  , c.[Name] [Company]
      , uc.[Usersid]
	  , u.[UUID] [UsersUUID]
	  , u.[firstname]
	  , u.[lastname]
	  , u.[IDNumber]
	  , u.[EmployeeNumber]
	  , u.[email]
      , uc.[UsersidCreated]
      , uc.[DateCreated]
      , uc.[DateStarted]
      , uc.[EmployeeNo]
      , uc.[ContactNumber]
      , uc.[isEmployee]
      , uc.[isActive]
      , uc.[isDeleted]
   , DATEDIFF(YEAR, uc.[DateStarted], GETDATE()) - CASE WHEN DATEFROMPARTS(YEAR(GETDATE()), MONTH(uc.[DateStarted])
	, CASE WHEN MONTH(uc.[DateStarted]) = 2 AND DAY(uc.[DateStarted]) = 29 THEN 28 ELSE DAY(uc.[DateStarted]) END) < GETDATE() THEN 0 ELSE 1 END [Years],
	DATEDIFF(MONTH, uc.[DateStarted], GETDATE()) % 12 AS [Months]
   FROM [base].[Userscompanys] uc
   INNER JOIN [dbo].[users] u ON u.[recordid] = uc.[Usersid] 
   INNER JOIN [dbo].[Companies] c ON c.[recordID] = uc.[Companyid] 
