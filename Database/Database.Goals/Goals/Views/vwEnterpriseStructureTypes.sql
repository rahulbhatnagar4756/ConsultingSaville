



CREATE VIEW [Goals].[vwEnterpriseStructureTypes] 
AS
SELECT  est.[Id]
, est.[UUID]
, est.[Companyid]
, c.[UUID] [CompanyUUID]
, c.[Name] [Company]
, est.[Name]
, est.[Description]
, est.[Iconsid]
, i.[Name] [Icons]
, est.[Color] [IconColor]
, est.[Orderval]
, est.[isDeleted]
FROM [Goals].[EnterpriseStructureTypes] est 
INNER JOIN [Base].[Companies] c ON c.[recordID] = est.[Companyid] 
LEFT OUTER JOIN [admin].[Icon] i ON i.[Id] = est.[Iconsid]