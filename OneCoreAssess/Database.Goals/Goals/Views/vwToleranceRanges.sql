

CREATE VIEW [Goals].[vwToleranceRanges]
AS
 SELECT r.[Id]
 , r.[UUID]
, ts.[Id] [ToleranceSetsid]
, ts.[UUID] [ToleranceSetsUUID]
, ts.[Companyid]
, c.[UUID] [CompanyidUUID]
, ts.[Name]
, r.[RangeStart]
, LEAD(r.[RangeStart]) OVER (ORDER BY r.[RangeStart]) AS [RangeEnd]
, r.[Score]
FROM [Goals].[ToleranceSets] ts
INNER JOIN [Base].[dbo].[Companies] c ON ts.[Companyid] = c.[recordID]
INNER JOIN [Goals].[ToleranceRanges] r ON r.[ToleranceSetsid] = ts.[Id] 
    AND r.[isDeleted] = 0
WHERE ts.[isDeleted] = 0
