CREATE PROCEDURE [Goals].[spEnterpriseStructureTypes_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @SecurityRoleAccessidDelete INT = 8 -- Goals Administration Delete
    , @isHasAccess BIT = 0
    , @AuditLogsid BIGINT
    , @EnterpriseStructureTypeId INT 

    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;

    IF @isHasAccess = 1
    BEGIN
        -- Use the views for lookups
        SELECT @CompanyId = [recordID] 
        FROM [Base].[Companies] 
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

        -- Verify the enterprise structure type belongs to the company
        IF NOT EXISTS (
            SELECT 1 
            FROM [Goals].[EnterpriseStructureTypes] est
            WHERE est.[UUID] = @UUID 
              AND est.[Companyid] = @CompanyId
        )
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Enterprise Structure Type not found or access denied' [Message]
            RETURN 0;
        END

        UPDATE [Goals].[EnterpriseStructureTypes]
        SET [isDeleted] = 1
        WHERE [UUID] = @UUID 
          AND [Companyid] = @CompanyId;
    
        SELECT @EnterpriseStructureTypeId = [Id]
        FROM [Goals].[EnterpriseStructureTypes]
        WHERE [UUID] = @UUID

        EXEC [audit].[spAuditLogs_Log_Delete] @CompanyId
                                            , @UsersId
                                            , 'EnterpriseStructureTypes'
                                            , @EnterpriseStructureTypeId 
                                            , NULL
                                            , @AuditLogsid OUTPUT

        SELECT @UUID [UUID]
        , 1 [isValid] 
        , 'Enterprise Structure Type deleted successfully' [Message]
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END

