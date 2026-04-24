
CREATE PROCEDURE [secure].[spSecurityRoles_ByUsersidCompanyid]
  @Usersid int 
, @Companyid int 
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Count INT = 0
	, @CompanyidDefault INT = 1

	DECLARE @Temp as table([Id] INT, [Name] NVARCHAR(300), [Description] NVARCHAR(MAX), [isDeleted] INT)

	INSERT INTO @Temp([Id], [Name], [Description], [isDeleted]) 
	SELECT r.[Id], r.[Name], r.[Description], r.[isDeleted]
	FROM [secure].[SecurityRoles] r
	INNER JOIN [secure].[SecurityUsersRoles] ur ON ur.[SecurityRolesid] = r.[Id] AND ur.[isDeleted] = 0 
											  AND ur.[Usersid] = @Usersid AND ur.[Companyid] = @Companyid 
	WHERE r.[isDeleted] = 0;

	--if user does not have any roles check the user againts the default company 
	IF NOT EXISTS(SELECT * FROM @Temp)
	BEGIN
		INSERT INTO @Temp([Id], [Name], [Description], [isDeleted])
		SELECT r.[Id], r.[Name], r.[Description], r.[isDeleted]
		FROM [secure].[SecurityRoles] r
		INNER JOIN [secure].[SecurityUsersRoles] ur ON ur.[SecurityRolesid] = r.[Id] AND ur.[isDeleted] = 0 
												  AND ur.[Usersid] = @Usersid AND ur.[Companyid] = @CompanyidDefault


	END

	--roles are connected to roles to make up several posibilities
	INSERT INTO @Temp([Id], [Name], [Description], [isDeleted]) 
	SELECT r.[Id], r.[Name], r.[Description], r.[isDeleted] 
	FROM [secure].[SecurityRoles] r
	INNER JOIN [secure].[SecurityRoleRoles] rr ON rr.[SecurityRolesidClone] = r.[Id]
	INNER JOIN @Temp t ON t.[Id] = rr.[SecurityRolesid]
	WHERE r.[isDeleted] = 0
	SELECT *
	FROM @Temp

END
