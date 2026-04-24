CREATE OR ALTER PROCEDURE [Goals].[sp_IsUserEmployee]
    @UserUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RoleName NVARCHAR(200);

    SELECT TOP 1
        @RoleName = r.Name
    FROM Base.dbo.Users u
    INNER JOIN Base.dbo.UsersRolesLinks rl
        ON rl.Usersid = u.recordid
       AND rl.isDeleted = 0
    INNER JOIN Base.dbo.UsersRoles r
        ON r.Id = rl.UsersRolesid
       AND r.isDeleted = 0
    WHERE u.UUID = @UserUUID;

    -- Return 1 if 'Employee', else 0
    IF @RoleName = 'EmployeeGoals'
        RETURN 1;
    ELSE
        RETURN 0;
END;
GO
