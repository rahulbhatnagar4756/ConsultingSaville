
CREATE PROCEDURE [Goals].[spKPALinks_Save]
(
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @ContractPeriodsUUID NVARCHAR(200),
    @KPALinkTypesid INT,
    @KPAUUID NVARCHAR(200),
    @KPALinksUUID NVARCHAR(200) = NULL,
    @LinkedUUID NVARCHAR(200),
    @Weight DECIMAL(18,4) = 100
)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersidLoggedIn INT
    , @ContractPeriodsId INT
    , @KPAId BIGINT
    , @LinkedId BIGINT
    , @KPALinksId INT
    , @ResultUUID NVARCHAR(200)
    , @IsValid BIT = 1
    , @Message NVARCHAR(MAX) = 'KPA Link saved successfully.'
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 7
    , @AuditLogsid BIGINT
    , @isUpdate BIT = 0
    -- Variables to capture old values for audit comparison
    , @OldWeight DECIMAL(18,4) = NULL
    , @OldLinkedId BIGINT = NULL
    , @KPALinkTypesidKPA INT = 1
    , @KPALinkTypesidKPI INT = 2


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
        
        -- Validate KPALinkTypesid (1=KPA, 2=KPI)
        IF @KPALinkTypesid NOT IN (1, 2)
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Invalid KPA Link Type. Must be 1 (KPA) or 2 (KPI).';
            GOTO ErrorHandler;
        END
        
        -- Validate and get KPAId
        SELECT @KPAId = Id 
        FROM [Goals].[KPA] 
        WHERE UUID = @KPAUUID 
            AND isDeleted = 0;

        IF @KPAId IS NULL
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Invalid KPA.';
            GOTO ErrorHandler;
        END
        
        -- Validate LinkedId based on KPALinkTypesid
        IF @KPALinkTypesid = @KPALinkTypesidKPA -- KPA
        BEGIN
            SELECT @LinkedId = Id 
            FROM [Goals].[KPA] 
            WHERE UUID = @LinkedUUID 
                AND isDeleted = 0;
        END
        ELSE IF @KPALinkTypesid = @KPALinkTypesidKPI -- KPI
        BEGIN
            SELECT @LinkedId = Id 
            FROM [Goals].[KPI] 
            WHERE UUID = @LinkedUUID 
                AND isDeleted = 0;
        END

        IF @LinkedId IS NULL
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Invalid Linked entity.';
            GOTO ErrorHandler;
        END
        
        -- Check Access Rights
        EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

        IF @isHasAccess = 1
        BEGIN
            BEGIN TRANSACTION;
            
            -- Check if updating existing record
            IF @KPALinksUUID IS NOT NULL AND @KPALinksUUID != ''
            BEGIN
                -- Capture current values for comparison
                SELECT @KPALinksId = Id 
                , @OldWeight = [Weight]
                , @OldLinkedId = [LinkedId]
                , @isUpdate = CASE WHEN [Weight] != @Weight OR
                                        [LinkedId] != @LinkedId THEN 1 ELSE 0 END
                FROM [Goals].[KPALinks] 
                WHERE UUID = @KPALinksUUID 
                    AND isDeleted = 0;
            
                IF @KPALinksId IS NOT NULL
                BEGIN
                    -- Update existing KPA Link
                    UPDATE [Goals].[KPALinks]
                    SET [KPALinkTypesid] = @KPALinkTypesid,
                        [LinkedId] = @LinkedId,
                        [Weight] = @Weight
                    WHERE Id = @KPALinksId;

                    -- Log the update if changes were made
                    IF @isUpdate = 1
                    BEGIN
                        EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                              @UsersidLoggedIn, 
                                                              'KPALinks', 
                                                              @KPALinksId, 
                                                              NULL, 
                                                              @AuditLogsid OUTPUT

                        -- Save only the values that have changed
                        IF @OldWeight != @Weight
                            EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Weight', @OldWeight

                        IF @OldLinkedId != @LinkedId
                            EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'LinkedId', @OldLinkedId
                    END
                
                    SET @ResultUUID = @KPALinksUUID;
                END
                ELSE
                BEGIN
                    SET @IsValid = 0;
                    SET @Message = 'KPA Link not found for update.';
                    GOTO ErrorHandler;
                END
            END
            ELSE
            BEGIN
                -- Check for duplicate link
                IF EXISTS (SELECT 1 FROM [Goals].[KPALinks] 
                          WHERE [KPAid] = @KPAId 
                            AND [LinkedId] = @LinkedId 
                            AND [KPALinkTypesid] = @KPALinkTypesid 
                            AND [isDeleted] = 0)
                BEGIN
                    SET @IsValid = 1;
                    SET @Message = 'Successfully added KPA Link.';
                    GOTO ErrorHandler;
                END

                -- Insert new KPA Link
                SET @ResultUUID = NEWID();
            
                INSERT INTO [Goals].[KPALinks] 
                (
                    [UUID],
                    [KPALinkTypesid],
                    [KPAid],
                    [LinkedId],
                    [Weight],
                    [isDeleted]
                )
                VALUES 
                (
                    @ResultUUID,
                    @KPALinkTypesid,
                    @KPAId,
                    @LinkedId,
                    @Weight,
                    0
                );

                SET @KPALinksId = SCOPE_IDENTITY();

                -- Log creation
                EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                      @UsersidLoggedIn, 
                                                      'KPALinks', 
                                                      @KPALinksId, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT
            END

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
        SET @Message = 'Error saving KPA Link: ' + ERROR_MESSAGE();
        SET @ResultUUID = NULL;
    END CATCH
    
    ErrorHandler:
    -- Return result
    SELECT 
        @ResultUUID AS [UUID],
        @IsValid AS [isValid],
        @Message AS [Message];
END