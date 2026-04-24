USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spScoringPeriods_Delete]    Script Date: 19/09/2025 11:36:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [Goals].[spScoringPeriods_Delete]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @ScoringPeriodsUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT,
            @SecurityRoleAccessidDelete INT = 8,
            @isHasAccess BIT = 0,
            @AuditLogsid BIGINT,
            @ScoringPeriodId BIGINT;

   
    ------------------------------------------------------------------------
    -- ACCESS CHECK SECTION
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
            SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message];
            RETURN 0;
        END

        -- Soft delete the Scoring Period
        UPDATE [Goals].[ScoringPeriods]
        SET [isDeleted] = 1,
            [isActive] = 0,
            [DateDeactivated] = GETDATE()
        WHERE [UUID] = @ScoringPeriodsUUID;

        -- Get Scoring Period ID for audit logging
        SELECT @ScoringPeriodId = [Id]
        FROM [Goals].[ScoringPeriods]
        WHERE [UUID] = @ScoringPeriodsUUID;

        -- Log the deletion in audit trail
        EXEC [audit].[spAuditLogs_Log_Delete_UUID] 
            @CompanyUUID,
            @UsersUUIDLoggedIn,
            'ScoringPeriods',
            @ScoringPeriodId,
            @AuditLogsid OUTPUT;

        -- Return success response
        SELECT @ScoringPeriodsUUID [UUID],
           @CompanyId AS [CompanyId],
               1 [isValid],
               'Scoring Period deleted successfully' [Message];

    
    ------------------------------------------------------------------------
    -- END OF ACCESS CHECK SECTION 
    ------------------------------------------------------------------------
    END
    ELSE
    BEGIN
        -- Return access denied response
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message];
        RETURN 0;
    END
    ------------------------------------------------------------------------
   
END
