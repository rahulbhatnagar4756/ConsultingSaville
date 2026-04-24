CREATE PROCEDURE [Goals].[spPillars_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @SecurityRoleAccessidDelete INT = 8
    , @isHasAccess BIT = 0
    , @AuditLogsid BIGINT
    , @Pillarsid INT 

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

        UPDATE [Goals].[Pillars]
        SET [isDeleted] = 1
        WHERE [UUID] = @UUID 
          AND [Companyid] = @CompanyId;
    
        SELECT @Pillarsid = [Id]
        FROM [Goals].[Pillars]
        WHERE [UUID] = @UUID

        EXEC [audit].[spAuditLogs_Log_Delete_UUID] @CompanyUUID
                                                 , @UsersUUIDLoggedIn
                                                 , 'Pillars'
                                                 , @Pillarsid 
                                                 , @AuditLogsid OUTPUT

        SELECT @UUID [UUID]
        , 1 [isValid] 
        , 'Pillar deleted successfully' [Message]
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END
