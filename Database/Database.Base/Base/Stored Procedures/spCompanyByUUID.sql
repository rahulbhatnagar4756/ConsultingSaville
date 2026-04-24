 
 
 
 CREATE PROCEDURE [base].[spCompanyByUUID]

   @CompanyUUID nvarchar(200) 
 , @UsersUUIDLoggedIn nvarchar(200) = ''
 AS
 BEGIN

	DECLARE @CompanySettingtypesPrimaryColor1 INT = 21
	, @CompanySettingtypesPrimaryColor2   INT = 22
	, @CompanySettingtypesSecondaryColor1 INT = 23
	, @CompanySettingtypesSecondaryColor2 INT = 24
	, @CompanySettingtypesSecondaryColor3 INT = 25
	, @CompanySettingtypesSecondaryColor4 INT = 26


	SELECT  c.[UUID] [CompanyUUID]
	, c.[Name]
	, c.[ReportCompanyLong] [Copyright]
	, c.[URLAddress] [URLAddress]
	, REPLACE(c.[Icon],'../',c.[URLAddress]) [CompanyLogoURL]
	, c.[DefaultLanguage] [Language]

	, ISNULL(s.[Value], '#0F1F38') [PrimaryColor1]
	, ISNULL(s2.[Value],'#727572') [PrimaryColor2]
	, ISNULL(ss1.[Value],'#FBDD7C') [SecondaryColor1]
	, ISNULL(ss2.[Value],'#CC6826') [SecondaryColor2]
	, ISNULL(ss3.[Value],'#7AB3E1') [SecondaryColor3]
	, ISNULL(ss4.[Value],'#616F85') [SecondaryColor4]
    
	FROM [dbo].[Companies] c
	LEFT OUTER JOIN [dbo].[CompanySettings] s ON s.[Companyid] = c.[recordID] AND s.[CompanySettingTypesid] = @CompanySettingtypesPrimaryColor1 AND s.[isDeleted] = 0
	LEFT OUTER JOIN [dbo].[CompanySettings] s2 ON s2.[Companyid] = c.[recordID] AND s2.[CompanySettingTypesid] = @CompanySettingtypesPrimaryColor1 AND s2.[isDeleted] = 0
  
	LEFT OUTER JOIN [dbo].[CompanySettings] ss1 ON ss1.[Companyid] = c.[recordID] AND ss1.[CompanySettingTypesid] = @CompanySettingtypesSecondaryColor1 AND ss1.[isDeleted] = 0
	LEFT OUTER JOIN [dbo].[CompanySettings] ss2 ON ss2.[Companyid] = c.[recordID] AND ss2.[CompanySettingTypesid] = @CompanySettingtypesSecondaryColor2 AND ss2.[isDeleted] = 0
	LEFT OUTER JOIN [dbo].[CompanySettings] ss3 ON ss3.[Companyid] = c.[recordID] AND ss3.[CompanySettingTypesid] = @CompanySettingtypesSecondaryColor3 AND ss3.[isDeleted] = 0
	LEFT OUTER JOIN [dbo].[CompanySettings] ss4 ON ss4.[Companyid] = c.[recordID] AND ss4.[CompanySettingTypesid] = @CompanySettingtypesSecondaryColor4 AND ss4.[isDeleted] = 0

	WHERE [UUID] = @CompanyUUID
 
 END


