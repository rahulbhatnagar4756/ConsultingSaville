CREATE PROCEDURE [Goals].[spStatus]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT s.[UUID],
        s.[Name],
        s.[Description],
        s.[OrderVal]
    FROM [Goals].[Status] s
    INNER JOIN [Base].[dbo].[Companies] c ON s.[Companyid] = c.[recordID]
    WHERE c.[UUID] = @CompanyUUID
      AND s.[isDeleted] = 0
    ORDER BY s.[OrderVal], s.[Name]
END
