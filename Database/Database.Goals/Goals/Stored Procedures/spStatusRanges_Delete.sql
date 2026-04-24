CREATE PROCEDURE [Goals].[spStatusRanges_Delete]
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
    , @StatusRangeId INT 

    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;

    IF @isHasAccess = 1
    BEGIN
        -- Use the new views for lookups
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

        -- Verify the status range belongs to the company
        IF NOT EXISTS (
            SELECT 1 
            FROM [Goals].[StatusRanges] sr
            INNER JOIN [Goals].[Status] s ON s.[Id] = sr.[Statusid] 
                AND s.[isDeleted] = 0
                AND s.[Companyid] = @CompanyId 
            WHERE sr.[UUID] = @UUID 
        )
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Status range not found or access denied' [Message]
            RETURN 0;
        END

        -- Check for child status ranges
        IF EXISTS (
            SELECT 1 
            FROM [Goals].[StatusRanges] sr
            INNER JOIN [Goals].[StatusRanges] parent ON sr.[StatusRangesid] = parent.[Id]
            WHERE parent.[UUID] = @UUID 
              AND sr.[isDeleted] = 0
        )
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Cannot delete status range that has child ranges. Please delete child ranges first.' [Message]
            RETURN 0;
        END

        UPDATE [Goals].[StatusRanges]
        SET [isDeleted] = 1
        WHERE [UUID] = @UUID;
    
        SELECT @StatusRangeId = [Id]
        FROM [Goals].[StatusRanges]
        WHERE [UUID] = @UUID

        EXEC [audit].[spAuditLogs_Log_Delete] @CompanyId
                                            , @UsersId
                                            , 'StatusRanges'
                                            , @StatusRangeId 
                                            , NULL
                                            , @AuditLogsid OUTPUT

        SELECT @UUID [UUID]
        , 1 [isValid] 
        , 'Status range deleted successfully' [Message]
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END

