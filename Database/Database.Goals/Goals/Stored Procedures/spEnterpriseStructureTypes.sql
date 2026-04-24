CREATE PROCEDURE [Goals].[spEnterpriseStructureTypes]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT est.[UUID],
        est.[Name],
        est.[Description],
        ISNULL(i.[Name], 'enterprise') as Icons,
        est.[Color]
    FROM [Goals].[EnterpriseStructureTypes] est
    INNER JOIN [Base].[Companies] c ON est.[Companyid] = c.[recordID]
    LEFT OUTER JOIN [admin].[Icon] i ON est.[Iconsid] = i.[Id]
    WHERE c.[UUID] = @CompanyUUID
      AND est.[isDeleted] = 0
    ORDER BY est.[Name]
END
