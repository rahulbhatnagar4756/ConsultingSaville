-- Status - Delete
CREATE PROCEDURE [Goals].[spStatus_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @SecurityRoleAccessidDelete INT = 8
    , @isHasAccess BIT = 0
    , @AuditLogsid BIGINT
    , @StatusId INT 

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

        SELECT TOP(1) @UsersId = [Id]
        FROM [Base].[Users]
        WHERE [UUID] = @UsersUUIDLoggedIn;

        IF ISNULL(@UsersId, 0) = 0
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Valid user not supplied' [Message]
            RETURN 0;
        END

        UPDATE [Goals].[Status]
        SET [isDeleted] = 1
        WHERE [UUID] = @UUID 
          AND [Companyid] = @CompanyId;
    
        SELECT @StatusId = [Id]
        FROM [Goals].[Status]
        WHERE [UUID] = @UUID

        EXEC [audit].[spAuditLogs_Log_Delete] @CompanyId
                                            , @UsersId
                                            , 'Status'
                                            , @StatusId 
                                            , NULL
                                            , @AuditLogsid OUTPUT

        SELECT @UUID [UUID]
        , 1 [isValid] 
        , 'Status deleted successfully' [Message]
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END

