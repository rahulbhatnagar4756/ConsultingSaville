

CREATE PROCEDURE [secure].[spSecurityUsersRoles_Has_Access]
	@UsersUUID NVARCHAR(200) 
	, @CompanyUUID NVARCHAR(200) 
	, @SecurityActionid int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @isValid bit = 0;

	SELECT @isValid = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END 
	FROM [secure].[vwSecurityUsersRoles] ur
	INNER JOIN [secure].[SecurityRoleActions] ra ON ra.[SecurityRolesid] = ur.[Id]
		AND ra.[isDeleted] = 0
		AND ra.[SecurityActionsid] = @SecurityActionid
	WHERE ur.[CompanyUUID] = @CompanyUUID
		AND ur.[UsersUUID] = @UsersUUID;

	RETURN @isValid;
END	
