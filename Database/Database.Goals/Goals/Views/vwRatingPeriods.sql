





CREATE VIEW [Goals].[vwRatingPeriods]
AS
SELECT  rp.[Id]
, rp.[UUID]
, rp.[Companyid]
, c.[UUID] [CompanyUUID]
, c.[Name] [Company]
, rp.[Name]
, rp.[DisplayName]
, rp.[isDeleted]
FROM [Goals].[RatingPeriods] rp
INNER JOIN [Base].[Companies] c ON c.[recordID] = rp.[Companyid]