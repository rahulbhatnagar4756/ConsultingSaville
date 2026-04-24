-- EnterpriseStructureTypes - Save (Insert/Update)
CREATE PROCEDURE [Goals].[spEnterpriseStructureTypes_Save]
    @UUID NVARCHAR(200) = NULL,
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @Name NVARCHAR(200),
    @Description NVARCHAR(MAX) = NULL,
    @Icons NVARCHAR(255) = 'enterprise',
    @Color NVARCHAR(50) = '#362f21'
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @EnterpriseStructureTypeId INT
    , @NewUUID NVARCHAR(200)
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 7 -- Goals Administration Edit
    , @AuditLogsid BIGINT
    , @IconsId INT = NULL
    , @isUpdate BIT = 0

    -- Use the views for lookups
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

            IF @UUID IS NULL OR @UUID = ''
            BEGIN
                -- Insert new record
                SET @NewUUID = NEWID();
            
                INSERT INTO [Goals].[EnterpriseStructureTypes] ([UUID], [Companyid], [Name], [Description], [Iconsid], [Color])
                VALUES (@NewUUID, @CompanyId, @Name, @Description, @IconsId, @Color);
            
                SET @EnterpriseStructureTypeId = SCOPE_IDENTITY();

                -- Log creation
                EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                      @UsersId, 
                                                      'EnterpriseStructureTypes', 
                                                      @EnterpriseStructureTypeId, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT
            
                SELECT [UUID]
                , 1 [isValid]
                , 'Enterprise Structure Type created successfully' as Message
                FROM [Goals].[EnterpriseStructureTypes]
                WHERE [Id] = @EnterpriseStructureTypeId; 
            END
            ELSE
            BEGIN
                -- Capture current values for comparison
                DECLARE @OldName NVARCHAR(200) = NULL
                , @OldDescription NVARCHAR(MAX) = NULL
                , @OldIconsId INT = NULL
                , @OldColor NVARCHAR(50) = NULL

                -- Verify the enterprise structure type belongs to the company before updating
                IF NOT EXISTS (
                    SELECT 1 
                    FROM [Goals].[EnterpriseStructureTypes] est
                    WHERE est.[UUID] = @UUID 
                      AND est.[Companyid] = @CompanyId
                      AND est.[isDeleted] = 0
                )
                BEGIN
                    ROLLBACK TRANSACTION;
                    SELECT NULL [UUID], 0 [isValid], 'Enterprise Structure Type not found or access denied' [Message]
                    RETURN 0;
                END

                -- Get current values and check for changes
                SELECT @EnterpriseStructureTypeId = [Id]
                , @OldName = [Name]
                , @OldDescription = [Description]
                , @OldIconsId = [Iconsid]
                , @OldColor = [Color]
                , @isUpdate = CASE WHEN [Name] != @Name OR
                                        ISNULL([Description], '') != ISNULL(@Description, '') OR
                                        ISNULL([Iconsid], 0) != ISNULL(@IconsId, 0) OR
                                        ISNULL([Color], '') != ISNULL(@Color, '') THEN 1 ELSE 0 END
                FROM [Goals].[EnterpriseStructureTypes]
                WHERE [UUID] = @UUID;

                IF @isUpdate = 1
                BEGIN
                    -- Update existing record
                    UPDATE [Goals].[EnterpriseStructureTypes]
                    SET [Name] = @Name,
                        [Description] = @Description,
                        [Iconsid] = @IconsId,
                        [Color] = @Color
                    WHERE [UUID] = @UUID;

                    -- Log the changes
                    EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                          @UsersId, 
                                                          'EnterpriseStructureTypes', 
                                                          @EnterpriseStructureTypeId, 
                                                          NULL, 
                                                          @AuditLogsid OUTPUT

                    -- Save only the values that have changed
                    IF @OldName != @Name
                        EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Name', @OldName

                    IF ISNULL(@OldDescription, '') != ISNULL(@Description, '')
                        EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Description', @OldDescription

                    IF ISNULL(@OldIconsId, 0) != ISNULL(@IconsId, 0)
                        EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Iconsid', @OldIconsId

                    IF ISNULL(@OldColor, '') != ISNULL(@Color, '')
                        EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Color', @OldColor
                END
            
                SELECT @UUID [UUID]
                , 1 as [isValid]
                , 'Enterprise Structure Type updated successfully' [Message];
            END

            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            
            SELECT NULL [UUID], 0 [isValid], 
                   'Error saving Enterprise Structure Type: ' + ERROR_MESSAGE() [Message];
            RETURN 0;
        END CATCH
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END
