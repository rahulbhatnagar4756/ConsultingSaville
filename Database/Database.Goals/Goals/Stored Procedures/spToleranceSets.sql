CREATE PROCEDURE [Goals].[spToleranceSets]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT ts.[UUID] 
    , ts.[Name]
    , ts.[Description]
    , ts.[MaxTotal]
    , ts.[OrderVal]
    , CASE WHEN COUNT(r.Id) = 0 THEN 'No' ELSE 'Yes' END [Ranges]
    FROM [Goals].[ToleranceSets] ts
    INNER JOIN [Base].[dbo].[Companies] c ON ts.[Companyid] = c.[recordID]
    LEFT OUTER JOIN [Goals].[ToleranceRanges] r ON r.[ToleranceSetsid] = ts.[Id] 
        AND r.[isDeleted] = 0
    WHERE c.[UUID] = @CompanyUUID
      AND ts.[isDeleted] = 0
    GROUP BY ts.[UUID] 
    , ts.[Name]
    , ts.[Description]
    , ts.[MaxTotal]
    , ts.[OrderVal]
    ORDER BY ts.[OrderVal], ts.[Name]

END
