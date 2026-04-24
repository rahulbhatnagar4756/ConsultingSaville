


CREATE VIEW [secure].[vwSecurityUsersRoles]
AS
SELECT sur.[Id]
, sur.[Companyid]
, c.[UUID] [CompanyUUID]
, sur.[Usersid]
, u.[UUID] [UsersUUID]
, sur.[SecurityRolesid]
, r.[Name] [SecurityRoles]
, ISNULL(r.[NameFriendly], r.[Name]) [SecurityRolesFriendly] 
, sur.[DateCreated]
, sur.[isDeleted]
FROM [secure].[SecurityUsersRoles] sur
INNER JOIN [dbo].[users] u ON u.[recordid] = sur.[Usersid] 
INNER JOIN [dbo].[Companies] c ON c.[recordID] = sur.[Companyid] 
INNER JOIN [secure].[SecurityRoles] r ON r.[Id] = sur.[SecurityRolesid]
