-- ToleranceRanges - Delete
CREATE PROCEDURE [Goals].[spToleranceRanges_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @Usersid INT
    , @SecurityRoleAccessidDelete INT = 8
    , @isHasAccess BIT = 0
    , @AuditLogsid BIGINT
    , @ToleranceRangesid INT 

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

        -- Verify the tolerance range belongs to the company through ToleranceSets
        IF NOT EXISTS (
            SELECT 1 
            FROM [Goals].[ToleranceRanges] tr
            INNER JOIN [Goals].[ToleranceSets] ts ON tr.[ToleranceSetsid] = ts.[Id]
            WHERE tr.[UUID] = @UUID 
              AND ts.[Companyid] = @CompanyId
        )
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Tolerance range not found or access denied' [Message]
            RETURN 0;
        END

        UPDATE [Goals].[ToleranceRanges]
        SET [isDeleted] = 1
        WHERE [UUID] = @UUID;
    
        SELECT @ToleranceRangesid = [Id]
        FROM [Goals].[ToleranceRanges]
        WHERE [UUID] = @UUID

        EXEC [audit].[spAuditLogs_Log_Delete_UUID] @CompanyUUID
                                                 , @UsersUUIDLoggedIn
                                                 , 'ToleranceRanges'
                                                 , @ToleranceRangesid 
                                                 , @AuditLogsid OUTPUT

        SELECT @UUID [UUID]
        , 1 [isValid] 
        , 'Tolerance range deleted successfully' [Message]
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END
