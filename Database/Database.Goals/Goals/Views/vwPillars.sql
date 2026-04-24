


CREATE VIEW [Goals].[vwPillars]
AS
SELECT p.[Id]
, p.[UUID]
, p.[Companyid]
, c.[UUID] [CompanyUUID]
, c.[Name] [Company]
, p.[Name]
, p.[Description]
, p.[IconsId]
, i.[Name] [Icons]
, p.[IconColor]
, p.[OrderVal]
, p.[isDeleted]
FROM [Goals].[Pillars] p
INNER JOIN [Base].[Companies] c ON c.[recordID] = p.[Companyid] 
LEFT OUTER JOIN [admin].[Icon] i ON i.[Id] = p.[Iconsid]