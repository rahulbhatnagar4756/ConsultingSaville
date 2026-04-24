create PROCEDURE [base].[spUserRolesByUUID]  
	@CompanyUUID NVARCHAR(200)
	, @UsersUUIDLoggedin NVARCHAR(200) = ''
	, @UsersUUID NVARCHAR(200) 
AS
BEGIN

	SELECT r.[Id]
	, r.[Name]
	FROM [dbo].[UsersRoles] r
	INNER JOIN [dbo].[UsersRolesLinks] rl ON rl.[UsersRolesid] = r.[Id] AND rl.[isDeleted] = 0
	INNER JOIN [dbo].[users] u ON u.[recordid] = rl.[Usersid] AND u.[UUID] = @UsersUUID
	INNER JOIN [dbo].[Companies] c ON c.[recordID] = rl.[Companyid] AND c.[UUID] = @CompanyUUID 

	WHERE r.[isDeleted] = 0

END

