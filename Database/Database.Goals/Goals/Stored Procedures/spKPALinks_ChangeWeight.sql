
CREATE PROCEDURE [Goals].[spKPALinks_ChangeWeight]
(
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @KPALinksUUID NVARCHAR(200),
    @NewWeight DECIMAL(18,4)
)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersidLoggedIn INT
    , @KPALinksId INT
    , @CurrentWeight DECIMAL(18,4)
    , @IsValid BIT = 1
    , @Message NVARCHAR(MAX) = 'KPA Link weight updated successfully.'
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 7
    , @AuditLogsid BIGINT

    BEGIN TRY
              
        -- Validate and get CompanyId
        SELECT @CompanyId = [recordID] 
        FROM [Base].[Companies] 
        WHERE UUID = @CompanyUUID 
            AND isDeleted = 0;

        IF @CompanyId IS NULL
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Invalid Company.';
            GOTO ErrorHandler;
        END
        
        -- Validate and get UsersId
        SELECT @UsersidLoggedIn = Id 
        FROM [Base].[Users] 
        WHERE UUID = @UsersUUIDLoggedIn 
            AND isDeleted = 0;

        IF @UsersidLoggedIn IS NULL
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Invalid User.';
            GOTO ErrorHandler;
        END
        
        -- Get KPA Link and current weight
        SELECT @KPALinksId = Id, @CurrentWeight = [Weight]
        FROM [Goals].[KPALinks] 
        WHERE UUID = @KPALinksUUID 
            AND isDeleted = 0;

        IF @KPALinksId IS NULL
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'KPA Link not found.';
            GOTO ErrorHandler;
        END

        -- Check if weight is actually changing
        IF @CurrentWeight = @NewWeight
        BEGIN
            SET @Message = 'Weight unchanged - no update needed.';
            GOTO SuccessHandler;
        END

         -- Check Access Rights
        EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

        IF @isHasAccess = 1
        BEGIN
            BEGIN TRANSACTION;
            
            -- Update weight
            UPDATE [Goals].[KPALinks]
            SET [Weight] = @NewWeight
            WHERE Id = @KPALinksId;

            -- Log the weight change
            EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                  @UsersidLoggedIn, 
                                                  'KPALinks', 
                                                  @KPALinksId, 
                                                  NULL, 
                                                  @AuditLogsid OUTPUT

            -- Save the old weight value
            EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Weight', @CurrentWeight

            COMMIT TRANSACTION;
        END
        ELSE
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Access denied.';
            GOTO ErrorHandler;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SET @IsValid = 0;
        SET @Message = 'Error updating KPA Link weight: ' + ERROR_MESSAGE();
    END CATCH
    
    SuccessHandler:
    ErrorHandler:
    -- Return result
    SELECT 
        CASE WHEN @IsValid = 1 THEN @KPALinksUUID ELSE NULL END AS [UUID],
        @IsValid AS [isValid],
        @Message AS [Message];
END