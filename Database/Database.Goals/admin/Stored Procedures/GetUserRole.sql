CREATE PROCEDURE [admin].[GetUserRole]
    @UsersUUID NVARCHAR(200),
    @CompanyUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

   Select R.Id AS ID,r.Name AS Name from  [BASE].[dbo].[UsersRoles] r
        INNER JOIN [BASE].[dbo].[UsersRolesLinks] rl 
            ON rl.[UsersRolesid] = r.[Id] 
           AND rl.[isDeleted] = 0
        INNER JOIN [BASE].[dbo].[users] u 
            ON u.[recordid] = rl.[Usersid] 
           AND u.[UUID] = @UsersUUID
        INNER JOIN [BASE].[dbo].[Companies] c 
            ON c.[recordID] = rl.[Companyid] 
           AND c.[UUID] = @CompanyUUID
        WHERE r.[isDeleted] = 0
END



--[admin].[GetUserRole1] 'EF8D91DC-F4B6-49EB-B8E7-FF349EEE9275','C1906F1D-38BE-40FF-A7D0-5C24B8AC5732'



--[dbo].[GetUserRole] 'C1906F1D-38BE-40FF-A7D0-5C24B8AC5732','EF8D91DC-F4B6-49EB-B8E7-FF349EEE9275'