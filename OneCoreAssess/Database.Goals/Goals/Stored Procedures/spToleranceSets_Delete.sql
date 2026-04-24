CREATE PROCEDURE [Goals].[spToleranceSets_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @SecurityRoleAccessidDelete INT = 6
    , @isHasAccess BIT = 0
    , @AuditLogsid BIGINT
    , @ToleranceSetsid int 

    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;

    IF @isHasAccess = 1
    BEGIN

        SELECT @CompanyId = [recordID] 
        FROM [Base].[dbo].[Companies] 
        WHERE [UUID] = @CompanyUUID;
    
        IF ISNULL(@Companyid, 0) = 0
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
            RETURN 0;
        END 

        UPDATE [Goals].[ToleranceSets]
        SET [isDeleted] = 1
        WHERE [UUID] = @UUID 
          AND [Companyid] = @CompanyId;
    
        SELECT @ToleranceSetsid = [Id]
        FROM [Goals].[ToleranceSets]
        WHERE [UUID] = @UUID

        EXEC [audit].[spAuditLogs_Log_Delete_UUID] @CompanyUUID
                                                 , @UsersUUIDLoggedIn
                                                 , 'ToleranceSets'
                                                 , @ToleranceSetsid 
                                                 , @AuditLogsid OUTPUT

        SELECT @UUID [UUID]
        , 1 [isValid] 
        , 'Tolerance Set deleted successfully'  [Message]
    END
END
