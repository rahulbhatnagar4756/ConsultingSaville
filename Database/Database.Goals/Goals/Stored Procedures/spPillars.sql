CREATE PROCEDURE [Goals].[spPillars]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.[UUID],
        p.[Name],
        p.[Description],
        ISNULL(i.[Name], 'account_balance') [Icons],
        p.[IconColor],
        p.[OrderVal]
    FROM [Goals].[Pillars] p
    INNER JOIN [Base].[dbo].[Companies] c ON p.[Companyid] = c.[recordID]
        AND c.[UUID] = @CompanyUUID
    LEFT OUTER JOIN [admin].[Icon] i ON i.[Id] = p.[IconsId]
    WHERE p.[isDeleted] = 0
    ORDER BY p.[OrderVal], p.[Name]
END
