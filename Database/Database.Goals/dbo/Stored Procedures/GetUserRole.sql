CREATE PROCEDURE [dbo].[GetUserRole]
    @UsersUUID NVARCHAR(200),
    @CompanyUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT r.[Id],
           r.[Name]
    FROM [BASE].[dbo].[UsersRoles] r
    INNER JOIN [BASE].[dbo].[UsersRolesLinks] rl 
        ON rl.[UsersRolesid] = r.[Id] 
       AND rl.[isDeleted] = 0
    INNER JOIN [BASE].[dbo].[users] u 
        ON u.[recordid] = rl.[Usersid] 
       AND u.[UUID] = @UsersUUID
    INNER JOIN [BASE].[dbo].[Companies] c 
        ON c.[recordID] = rl.[Companyid] 
       AND c.[UUID] = @CompanyUUID
    WHERE r.[isDeleted] = 0;
END