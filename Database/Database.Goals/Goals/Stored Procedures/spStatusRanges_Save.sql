CREATE   PROCEDURE [Goals].[spStatusRanges_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200) = NULL,
    @StatusUUID NVARCHAR(200), 
    @Name VARCHAR(255),
    @Icons NVARCHAR(255) = 'clock_loader_40',
    @Color NVARCHAR(50) = '#362f21',
    @RangeStart DECIMAL(10,2) = NULL, 
    @isNegative INT = 0,
    @OrderVal INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @StatusRangeId INT
    , @StatusId INT
    , @ParentStatusRangeId INT = NULL
    , @NewUUID NVARCHAR(200)
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 7 -- Goals Administration Edit
    , @AuditLogsid BIGINT
    , @IconsId INT = NULL
    , @isUpdate BIT = 0

    -- Use the new views for lookups
    SELECT @CompanyId = [recordID] 
    FROM [Base].[Companies] 
    WHERE [UUID] = @CompanyUUID;
    
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
        RETURN 0;
    END

    SELECT TOP(1) @UsersId = [Id]
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    IF ISNULL(@UsersId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid user not supplied' [Message]
        RETURN 0;
    END

    -- Get Status ID and verify company ownership
    SELECT @StatusId = [Id]
    FROM [Goals].[Status]
    WHERE [UUID] = @StatusUUID 
      AND [Companyid] = @CompanyId
      AND [isDeleted] = 0;

    IF ISNULL(@StatusId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Invalid status or access denied' [Message]
        RETURN 0;
    END
     

    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

    IF @isHasAccess = 1
    BEGIN
        BEGIN TRANSACTION;
        
        BEGIN TRY
            -- Get Icon ID if provided
            IF @Icons IS NOT NULL AND @Icons != ''
            BEGIN
                SELECT TOP(1) @IconsId = [Id]
                FROM [admin].[Icon]
                WHERE [Name] = @Icons;
            END

            -- Auto-assign OrderVal if not provided
            IF @OrderVal = 0
                SELECT @OrderVal = ISNULL(MAX(sr.[Orderval]), 0) + 1
                FROM [Goals].[StatusRanges] sr
                WHERE sr.[Statusid] = @StatusId
                  AND sr.[isDeleted] = 0

            IF @UUID IS NULL OR @UUID = ''
            BEGIN
                -- Insert new record (removed Companyid from INSERT)
                SET @NewUUID = NEWID();
            
                INSERT INTO [Goals].[StatusRanges] ([UUID], [Statusid], [StatusRangesid], [Name], [Iconsid], [Color], [RangeStart], [isNegative], [Orderval])
                VALUES (@NewUUID, @StatusId, @ParentStatusRangeId, @Name, @IconsId, @Color, @RangeStart, @isNegative, @OrderVal);
            
                SET @StatusRangeId = SCOPE_IDENTITY();

                -- Log creation
                EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                      @UsersId, 
                                                      'StatusRanges', 
                                                      @StatusRangeId, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT
            
                SELECT [UUID]
                , 1 [isValid]
                , 'Status range created successfully' as Message
                FROM [Goals].[StatusRanges]
                WHERE [Id] = @StatusRangeId; 
            END
            ELSE
            BEGIN
                -- Capture current values for comparison
                DECLARE @OldName VARCHAR(255) = NULL
                , @OldIconsId INT = NULL
                , @OldColor NVARCHAR(50) = NULL
                , @OldRangeStart DECIMAL(10,2) = NULL
                , @OldValue DECIMAL(10,2) = NULL
                , @OldIsNegative INT = NULL
                , @OldOrderVal INT = NULL

                -- Verify the status range belongs to the company before updating (via Status table)
                IF NOT EXISTS (
                    SELECT 1 
                    FROM [Goals].[StatusRanges] sr
                    INNER JOIN [Goals].[Status] s ON sr.[Statusid] = s.[Id]
                        AND s.[Companyid] = @CompanyId
                        AND s.[isDeleted] = 0
                    WHERE sr.[UUID] = @UUID 
                      AND sr.[isDeleted] = 0
                )
                BEGIN
                    ROLLBACK TRANSACTION;
                    SELECT NULL [UUID], 0 [isValid], 'Status range not found or access denied' [Message]
                    RETURN 0;
                END

                -- Get current values and check for changes
                SELECT @StatusRangeId = [Id]
                , @OldName = [Name]
                , @OldIconsId = [Iconsid]
                , @OldColor = [Color]
                , @OldRangeStart = [RangeStart] 
                , @OldIsNegative = [isNegative]
                , @OldOrderVal = [Orderval]
                , @isUpdate = CASE WHEN [Name] != @Name OR
                                        ISNULL([Iconsid], 0) != ISNULL(@IconsId, 0) OR
                                        [Color] != @Color OR
                                        ISNULL([RangeStart], 0) != ISNULL(@RangeStart, 0) OR 
                                        ISNULL([isNegative], 0) != @isNegative OR
                                        [Orderval] != @OrderVal THEN 1 ELSE 0 END
                FROM [Goals].[StatusRanges]
                WHERE [UUID] = @UUID;

                IF @isUpdate = 1
                BEGIN
                    -- Update existing record
                    UPDATE [Goals].[StatusRanges]
                    SET [Name] = @Name,
                        [Iconsid] = @IconsId,
                        [Color] = @Color,
                        [RangeStart] = @RangeStart, 
                        [isNegative] = @isNegative,
                        [Orderval] = @OrderVal
                    WHERE [UUID] = @UUID;

                    -- Log the changes
                    EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                          @UsersId, 
                                                          'StatusRanges', 
                                                          @StatusRangeId, 
                                                          NULL, 
                                                          @AuditLogsid OUTPUT

                    -- Save only the values that have changed
                    IF @OldName != @Name
                        EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Name', @OldName

                    IF ISNULL(@OldIconsId, 0) != ISNULL(@IconsId, 0)
                        EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Iconsid', @OldIconsId

                    IF @OldColor != @Color
                        EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Color', @OldColor

                    IF ISNULL(@OldRangeStart, 0) != ISNULL(@RangeStart, 0)
                        EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'RangeStart', @OldRangeStart
 

                    IF ISNULL(@OldIsNegative, 0) != @isNegative
                        EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'isNegative', @OldIsNegative

                    IF @OldOrderVal != @OrderVal
                        EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Orderval', @OldOrderVal
                END
            
                SELECT @UUID [UUID]
                , 1 as [isValid]
                , 'Status range updated successfully' [Message];
            END

            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            
            SELECT NULL [UUID], 0 [isValid], 
                   'Error saving status range: ' + ERROR_MESSAGE() [Message];
            RETURN 0;
        END CATCH
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END
