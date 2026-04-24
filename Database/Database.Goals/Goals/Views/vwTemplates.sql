

CREATE VIEW [Goals].[vwTemplates]
AS
SELECT  t.[Id]
, t.[UUID]
, t.[Companyid]
, c.[UUID] [CompanyUUID]
, c.[Name] [Company]
, t.[DateCreated]
, t.[UsersIdCreated]
, t.[Name]
, t.[Code]
, t.[isDeleted]
FROM [Goals].[Templates] t
INNER JOIN [Base].[Companies] c ON c.[recordID] = t.[Companyid] 

