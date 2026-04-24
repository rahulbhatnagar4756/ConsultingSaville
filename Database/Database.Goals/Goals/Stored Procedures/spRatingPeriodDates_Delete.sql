USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spRatingPeriodDates_Delete]    Script Date: 18/09/2025 15:09:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Goals].[spRatingPeriodDates_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @RatingPeriodDatesUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT,
            @SecurityRoleAccessidDelete INT = 8,
            @isHasAccess BIT = 0,
            @AuditLogsid BIGINT,
            @RatingPeriodDatesid INT;
    
 
    ------------------------------------------------------------------------
    -- ACCESS CHECK SECTION (Commented temporarily)
    ------------------------------------------------------------------------
    -- Check user access permissions
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] 
        @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidDelete;
    
    IF @isHasAccess = 1
    BEGIN
    ------------------------------------------------------------------------
  
    
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
        
        -- Soft delete the Rating Period Date
        UPDATE rpd
        SET rpd.[isDeleted] = 1,
            rpd.[isActive] = 0
        FROM [Goals].[RatingPeriodDates] rpd
        INNER JOIN [Goals].[RatingPeriods] rp ON rpd.[RatingPeriodsid] = rp.[Id]
        WHERE rpd.[UUID] = @RatingPeriodDatesUUID 
          AND rp.[Companyid] = @CompanyId;
    
        -- Get Rating Period Date ID for audit logging
        SELECT @RatingPeriodDatesid = rpd.[Id]
        FROM [Goals].[RatingPeriodDates] rpd
        INNER JOIN [Goals].[RatingPeriods] rp ON rpd.[RatingPeriodsid] = rp.[Id]
        WHERE rpd.[UUID] = @RatingPeriodDatesUUID
          AND rp.[Companyid] = @CompanyId;
        
        -- Log the deletion in audit trail
        EXEC [audit].[spAuditLogs_Log_Delete_UUID] @CompanyUUID,
                                                   @UsersUUIDLoggedIn,
                                                   'RatingPeriodDates',
                                                   @RatingPeriodDatesid,
                                                   @AuditLogsid OUTPUT;
        
        -- Return success response
        SELECT @RatingPeriodDatesUUID [UUID],
               1 [isValid],
               'Rating Period Date deleted successfully' [Message];
    

    ------------------------------------------------------------------------
    -- END OF ACCESS CHECK SECTION (Commented temporarily)
    ------------------------------------------------------------------------
    END
    ELSE
    BEGIN
        -- Return access denied response
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
    ------------------------------------------------------------------------
  
END