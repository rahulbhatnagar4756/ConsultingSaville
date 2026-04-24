


CREATE PROCEDURE [Goals].[spToleranceRanges]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @ToleranceSetsUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
   SELECT 
    tr.[UUID],
    ts.[UUID] [ToleranceSetsUUID],
    tr.[RangeStart],
    LEAD(tr.[RangeStart]) OVER (ORDER BY tr.[RangeStart]) AS [RangeEnd],
    tr.[Score] 
    FROM [Goals].[ToleranceRanges] tr
    
    INNER JOIN [Goals].[ToleranceSets] ts ON tr.[ToleranceSetsid] = ts.[Id]
        AND ts.[UUID] = @ToleranceSetsUUID
        AND ts.[isDeleted] = 0
    
    INNER JOIN [Base].[dbo].[Companies] c ON ts.[Companyid] = c.[recordID]
        AND c.[UUID] = @CompanyUUID
    
    WHERE tr.[isDeleted] = 0
    ORDER BY CASE WHEN tr.[RangeStart] IS NULL THEN 1 ELSE 2 END
        , tr.[RangeStart]

END
