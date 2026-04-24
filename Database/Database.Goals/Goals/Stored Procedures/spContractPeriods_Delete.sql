CREATE PROCEDURE [Goals].[spContractPeriods_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @ContractPeriodsUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT,
            @SecurityRoleAccessidDelete INT = 8,
            @isHasAccess BIT = 0,
            @AuditLogsid BIGINT,
            @ContractPeriodsid INT;
 
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] 
        @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;
    
    IF @isHasAccess = 1
    BEGIN
    
        -- Get Company ID from UUID
        SELECT @CompanyId = [recordID] 
        FROM [Base].[dbo].[Companies] 
        WHERE [UUID] = @CompanyUUID;
    
        -- Validate Company ID
        IF ISNULL(@CompanyId, 0) = 0
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
            RETURN 0;
        END 
        
        -- Soft delete the Contract Period
        UPDATE [Goals].[ContractPeriods]
        SET [isDeleted] = 1,
            [isActive] = 0
        WHERE [UUID] = @ContractPeriodsUUID 
          AND [Companyid] = @CompanyId;
    
        -- Get Contract Period ID for audit logging
        SELECT @ContractPeriodsid = [Id]
        FROM [Goals].[ContractPeriods]
        WHERE [UUID] = @ContractPeriodsUUID
          AND [Companyid] = @CompanyId;
        
        -- Log the deletion in audit trail
        EXEC [audit].[spAuditLogs_Log_Delete_UUID] @CompanyUUID,
                                                   @UsersUUIDLoggedIn,
                                                   'ContractPeriods',
                                                   @ContractPeriodsid,
                                                   @AuditLogsid OUTPUT;
        
        -- Return success response
        SELECT @ContractPeriodsUUID [UUID],
               1 [isValid],
               'Contract Period deleted successfully' [Message];
    END
    ELSE
    BEGIN
        -- Return access denied response
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END