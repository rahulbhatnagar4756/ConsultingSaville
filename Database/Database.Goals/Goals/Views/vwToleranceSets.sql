
CREATE VIEW [Goals].[vwToleranceSets]
AS
SELECT  ts.[Id]
, ts.[UUID]
, ts.[Companyid]
, c.[UUID] [CompanyUUID]
, ts.[Name]
, ts.[Description]
, ts.[MaxTotal]
, ts.[OrderVal]
, ts.[isDeleted]
FROM [Goals].[Goals].[ToleranceSets] ts
INNER JOIN [Base].[dbo].[Companies] c ON c.[recordID] = ts.[Companyid]
