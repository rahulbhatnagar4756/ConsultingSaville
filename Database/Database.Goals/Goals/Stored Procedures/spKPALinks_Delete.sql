
CREATE PROCEDURE [Goals].[spKPALinks_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @KPALinksUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @KPALinksId INT
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidDelete INT = 8 -- Assuming 8 is delete permission
    , @AuditLogsid BIGINT

    -- Get Company ID
    SELECT @CompanyId = [recordID] 
    FROM [Base].[Companies] 
    WHERE [UUID] = @CompanyUUID 
        AND [isDeleted] = 0;
    
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
        RETURN 0;
    END

    -- Get User ID
    SELECT TOP(1) @UsersId = [Id]
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn
        AND [isDeleted] = 0;

    IF ISNULL(@UsersId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid user not supplied' [Message]
        RETURN 0;
    END

    -- Check if KPA Link exists and get its ID
    SELECT @KPALinksId = [Id]
    FROM [Goals].[KPALinks]
    WHERE [UUID] = @KPALinksUUID 
      AND [isDeleted] = 0;

    IF @KPALinksId IS NULL
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'KPA Link not found or already deleted' [Message]
        RETURN 0;
    END

    -- Check Access Rights
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;

    IF @isHasAccess = 1
    BEGIN
        BEGIN TRY
            BEGIN TRANSACTION;

            -- Perform soft delete
            UPDATE [Goals].[KPALinks]
            SET [isDeleted] = 1
            WHERE [Id] = @KPALinksId;

            -- Log the deletion
            EXEC [audit].[spAuditLogs_Log_Delete] @CompanyId, 
                                                  @UsersId, 
                                                  'KPALinks', 
                                                  @KPALinksId, 
                                                  NULL, 
                                                  @AuditLogsid OUTPUT

            COMMIT TRANSACTION;
            SELECT @KPALinksUUID [UUID], 1 [isValid], 'KPA Link deleted successfully' [Message];
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;
            SELECT NULL [UUID], 0 [isValid], 'Error deleting KPA Link: ' + ERROR_MESSAGE() [Message];
        END CATCH
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END