USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spRatingPeriodDates_UpdateIsActive]    Script Date: 18/09/2025 15:11:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER   PROCEDURE [Goals].[spRatingPeriodDates_UpdateIsActive]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @RatingPeriodDatesUUID NVARCHAR(200),
    @IsActive BIT  -- 1 = Activate, 0 = Deactivate
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT,
            @SecurityRoleAccessidUpdate INT = 9, -- Example: Update permission role
            @isHasAccess BIT = 0,
            @AuditLogsid BIGINT,
            @RatingPeriodDatesid INT;
    
  
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] 
       @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidUpdate;
    
    IF @isHasAccess = 1
    BEGIN
    
        -- Get Company ID from UUID
        SELECT @CompanyId = [recordID] 
        FROM [Base].[dbo].[Companies] 
        WHERE [UUID] = @CompanyUUID;
    
        -- Validate Company ID
        IF ISNULL(@CompanyId, 0) = 0
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message];
            RETURN 0;
        END 
        
        -- Update isActive flag
        UPDATE rpd
        SET rpd.[isActive] = @IsActive
        FROM [Goals].[RatingPeriodDates] rpd
        INNER JOIN [Goals].[RatingPeriods] rp ON rpd.[RatingPeriodsid] = rp.[Id]
        WHERE rpd.[UUID] = @RatingPeriodDatesUUID 
          AND rp.[Companyid] = @CompanyId
          AND rpd.[isDeleted] = 0;  -- Avoid updating deleted records
    
        -- Get Rating Period Date ID for audit logging
        SELECT @RatingPeriodDatesid = rpd.[Id]
        FROM [Goals].[RatingPeriodDates] rpd
        INNER JOIN [Goals].[RatingPeriods] rp ON rpd.[RatingPeriodsid] = rp.[Id]
        WHERE rpd.[UUID] = @RatingPeriodDatesUUID
          AND rp.[Companyid] = @CompanyId;
        
        -- Log the update in audit trail
        EXEC [audit].[spAuditLogs_Log_Update_UUID] @CompanyUUID,
                                                   @UsersUUIDLoggedIn,
                                                   'RatingPeriodDates',
                                                   @RatingPeriodDatesid,
                                                   @AuditLogsid OUTPUT;
        
        -- Return success response
        SELECT @RatingPeriodDatesUUID [UUID],
               1 [isValid],
               CASE 
                    WHEN @IsActive = 1 THEN 'Rating Period Date activated successfully'
                    ELSE 'Rating Period Date deactivated successfully'
               END [Message];
    
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END
