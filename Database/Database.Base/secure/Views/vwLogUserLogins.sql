




CREATE VIEW [Secure].[vwLogUserLogins]
AS
SELECT ROW_NUMBER() OVER(PARTITION BY uc.[Usersid] ORDER BY uc.[DateLogged] DESC ) [No]
, uc.[Id]
, uc.[Companyid]
, c.[UUID] [CompanyUUID]
, c.[Name] [Company]
, uc.[Usersid]
, u.[UUID] [UsersUUID]
, u.[firstname] 
, u.[lastname]
, u.[IDNumber]
, u.[email]
, uc.[DateLogged]
, uc.[Statusid]

FROM [Secure].[LogUserLogins] uc
INNER JOIN [dbo].[Companies] c ON c.[recordID] = uc.[Companyid] 
INNER JOIN [dbo].[users] u ON u.[recordid] = uc.[Usersid] 
