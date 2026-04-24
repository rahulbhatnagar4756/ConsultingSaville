CREATE PROCEDURE [Goals].[spRatingPeriods_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @RatingPeriodsUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT,
            @SecurityRoleAccessidDelete INT = 8,
            @isHasAccess BIT = 0,
            @AuditLogsid BIGINT,
            @RatingPeriodsid INT;

    -- Check user access permissions
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;
    
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
        
        -- Soft delete the Rating Period
        UPDATE [Goals].[RatingPeriods]
        SET [isDeleted] = 1
        WHERE [UUID] = @RatingPeriodsUUID 
          AND [Companyid] = @CompanyId;
    
        -- Get Rating Period ID for audit logging
        SELECT @RatingPeriodsid = [Id]
        FROM [Goals].[RatingPeriods]
        WHERE [UUID] = @RatingPeriodsUUID
          AND [Companyid] = @CompanyId;
        
        -- Log the deletion in audit trail
        EXEC [audit].[spAuditLogs_Log_Delete_UUID] @CompanyUUID,
                                                   @UsersUUIDLoggedIn,
                                                   'RatingPeriods',
                                                   @RatingPeriodsid,
                                                   @AuditLogsid OUTPUT;
        
        -- Return success response
        SELECT @RatingPeriodsUUID [UUID],
             @CompanyId AS [CompanyId],
               1 [isValid],
               'Rating Period deleted successfully' [Message];
    
    END
    ELSE
    BEGIN
        -- Return access denied response
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
    
END