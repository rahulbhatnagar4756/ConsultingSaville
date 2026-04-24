CREATE PROCEDURE [Base].[spUsers_Basic_Delete]
    @CompanyUUID        NVARCHAR(200),
    @UsersUUIDLoggedIn  NVARCHAR(200),
    @UUID               NVARCHAR(200) 
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @SecurityRoleAccessidDelete INT = 11
    , @isHasAccess BIT = 0
    , @AuditLogsid BIGINT
    , @Usersid INT
    , @UsersidLoggedIn INT

    -- Initialize outputs 
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;

    IF @isHasAccess = 1
    BEGIN
        SELECT @CompanyId = [recordID] 
        FROM [Base].[dbo].[Companies] 
        WHERE [UUID] = @CompanyUUID;
    
        IF ISNULL(@CompanyId, 0) = 0
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
            RETURN 0;
        END 

        -- Get logged in user ID
        SELECT TOP(1) @UsersidLoggedIn = [recordid]
        FROM [Base].[dbo].[users]
        WHERE [UUID] = @UsersUUIDLoggedIn;

        -- Get user to delete and validate exists
        SELECT @Usersid = [recordid]
        FROM [Base].[dbo].[users]
        WHERE [UUID] = @UUID;

        IF @Usersid IS NULL
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'User not found' [Message]
            RETURN 0;
        END

        -- Security check: prevent self-deletion
        IF @Usersid = @UsersidLoggedIn
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'You cannot delete your own account' [Message]
            RETURN 0;
        END

        -- Soft delete 
        UPDATE [Base].[dbo].[users]
        SET [isDeleted] = 1 
        WHERE [UUID] = @UUID;
        
        -- Log deletion
        EXEC [audit].[spAuditLogs_Log_Delete_UUID] @CompanyUUID
                                                 , @UsersUUIDLoggedIn
                                                 , 'users'
                                                 , @Usersid 
                                                 , @AuditLogsid OUTPUT;

        SELECT @UUID [UUID], 1 [isValid], 'User deleted successfully' [Message]
        RETURN 1;
    END
    ELSE
    BEGIN
        SELECT @UUID [UUID], 1 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END