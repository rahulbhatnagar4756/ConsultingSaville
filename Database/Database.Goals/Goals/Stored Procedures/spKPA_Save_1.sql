
CREATE     PROCEDURE [Goals].[spKPA_Save]
(
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @KPAUUID NVARCHAR(200) = NULL,
    @ContractPillarsUUID NVARCHAR(200),
    @StatusUUID NVARCHAR(200),
    @RatingPeriodsUUID NVARCHAR(200),
    @Name NVARCHAR(500),
    @Description NVARCHAR(MAX) = NULL,
    @Weight DECIMAL(18,4),
    @isActive BIT = 1
)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersidLoggedIn INT
    , @StatusId INT
    , @RatingPeriodsid INT = NULL
    , @KPAId INT
    , @ResultUUID NVARCHAR(200)
    , @IsValid BIT = 1
    , @Message NVARCHAR(MAX) = 'KPA saved successfully.'
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 7
    , @ContractPillarsId INT
    , @isSuccessful BIT = 0 
    , @AuditLogsid BIGINT
    , @isUpdate BIT = 0
    -- Variables to capture old values for audit comparison
    , @OldStatusId INT = NULL
    , @OldRatingPeriodsid INT = NULL
    , @OldName NVARCHAR(500) = NULL
    , @OldDescription NVARCHAR(MAX) = NULL
    , @OldIsActive BIT = NULL

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
        
        -- Validate and get StatusId
        SELECT @StatusId = Id 
        FROM [Goals].[Status] 
        WHERE UUID = @StatusUUID 
            AND isDeleted = 0;

        IF @StatusId IS NULL
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Invalid Status.';
            GOTO ErrorHandler;
        END
        
        -- Validate Name
        IF LTRIM(RTRIM(ISNULL(@Name, ''))) = ''
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'KPA Name is required.';
            GOTO ErrorHandler;
        END
        
        SELECT @ContractPillarsId = Id 
        FROM [Goals].[ContractPillars] 
        WHERE UUID = @ContractPillarsUUID 
            AND isDeleted = 0;

        IF @ContractPillarsId IS NULL
        BEGIN
                
            SET @IsValid = 0;
            SET @Message = 'Invalid Contract Pillar.';
            GOTO ErrorHandler;
        END

        IF @RatingPeriodsUUID IS NOT NULL
        BEGIN
            SELECT @RatingPeriodsid = s.[Id]
            FROM [Goals].[RatingPeriods] s
            WHERE s.[UUID] = @RatingPeriodsUUID
                AND s.[isDeleted] = 0

            IF @RatingPeriodsid IS NULL
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'Valid rating period not found' [Message]
                RETURN 0;
            END
        END

         -- Check Access Rights
        EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

        IF @isHasAccess = 1
        BEGIN
            BEGIN TRANSACTION;
            -- Check if updating existing record
            IF @KPAUUID IS NOT NULL AND @KPAUUID != ''
            BEGIN

                -- Capture current values for comparison
                SELECT @KPAId = Id 
                , @OldStatusId = [Statusid]
                , @OldRatingPeriodsid = [RatingPeriodsid]
                , @OldName = [Name]
                , @OldDescription = [Description]
                , @OldIsActive = [isActive]
                , @isUpdate = CASE WHEN [Statusid] != @StatusId OR
                                        ISNULL([RatingPeriodsid], 0) != ISNULL(@RatingPeriodsid, 0) OR
                                        [Name] != @Name OR
                                        ISNULL([Description], '') != ISNULL(@Description, '') OR
                                        [isActive] != @isActive THEN 1 ELSE 0 END
                FROM [Goals].[KPA] 
                WHERE UUID = @KPAUUID 
                    AND isDeleted = 0;
            
                IF @KPAId IS NOT NULL
                BEGIN
                    -- Update existing KPA
                    UPDATE [Goals].[KPA]
                    SET [Statusid] = @StatusId,
                        [RatingPeriodsid] = @RatingPeriodsid,
                        [Name] = @Name,
                        [Description] = @Description, 
                        [isActive] = @isActive
                    WHERE Id = @KPAId;

                    -- Log the update if changes were made
                    IF @isUpdate = 1
                    BEGIN
                        EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                              @UsersidLoggedIn, 
                                                              'KPA', 
                                                              @KPAId, 
                                                              NULL, 
                                                              @AuditLogsid OUTPUT

                        -- Save only the values that have changed
                        IF @OldStatusId != @StatusId
                            EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Statusid', @OldStatusId

                        IF ISNULL(@OldRatingPeriodsid, 0) != ISNULL(@RatingPeriodsid, 0)
                            EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'RatingPeriodsid', @OldRatingPeriodsid

                        IF @OldName != @Name
                            EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Name', @OldName

                        IF ISNULL(@OldDescription, '') != ISNULL(@Description, '')
                            EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Description', @OldDescription

                        IF @OldIsActive != @isActive
                            EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'isActive', @OldIsActive
                    END
                
                    SET @ResultUUID = @KPAUUID;
                END
                ELSE
                BEGIN
                    SET @IsValid = 0;
                    SET @Message = 'KPA not found for update.';
                    GOTO ErrorHandler;
                END
            END
            ELSE
            BEGIN
                -- Insert new KPA
                SET @ResultUUID = NEWID();
            
                INSERT INTO [Goals].[KPA] 
                (
                    [UUID],
                    [CompanyId],
                    [Usersid],
                    [Statusid], 
                    [RatingPeriodsid],
                    [Name],
                    [Description],
                    [isActive]
                )
                VALUES 
                (
                    @ResultUUID,
                    @CompanyId,
                    @UsersidLoggedIn,
                    @StatusId, 
                    @RatingPeriodsid,
                    @Name,
                    @Description, 
                    @isActive 
                );

                SET @KPAid = SCOPE_IDENTITY();

                -- Log creation
                EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                      @UsersidLoggedIn, 
                                                      'KPA', 
                                                      @KPAid, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT

            END
        
            EXEC [Goals].[spContractPillarKPAs_Save] @companyid, @UsersidLoggedIn, NULL, @ContractPillarsId, @KPAid, @Weight, @isSuccessful OUTPUT, @Message OUTPUT
            IF @isSuccessful = 0
            BEGIN
                ROLLBACK TRANSACTION;
                GOTO ErrorHandler;
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
            ROLLBACK TRANSACTION;
            SET @IsValid = 0;
            SET @Message = 'Error saving KPA: ' + ERROR_MESSAGE();
            SET @ResultUUID = NULL;
        END CATCH
    ErrorHandler:

    -- Return result
    SELECT 
        @ResultUUID AS [UUID],
        @IsValid AS [isValid],
        @Message AS [Message];
END