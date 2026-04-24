
-- Templates - Save (Insert/Update)
CREATE   PROCEDURE [Goals].[spTemplates_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200) = NULL,
    @Name NVARCHAR(500),
    @Code NVARCHAR(500) = NULL,
    @Description NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @TemplateId INT
    , @NewUUID NVARCHAR(200)
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 7
    , @AuditLogsid BIGINT
    , @isUpdate BIT = 0

    -- Get Company ID
    SELECT @CompanyId = [recordID] 
    FROM [Base].[dbo].[Companies] 
    WHERE [UUID] = @CompanyUUID;
    
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
        RETURN 0;
    END

    -- Get User ID
    SELECT TOP(1) @UsersId = [Id]
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    IF ISNULL(@UsersId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid user not supplied' [Message]
        RETURN 0;
    END

    -- Check Access Rights
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

    IF @isHasAccess = 1
    BEGIN
        -- Check for duplicate Code within company (if Code is provided)
        IF @Code IS NOT NULL AND @Code != ''
        BEGIN
            DECLARE @ExistingTemplateId INT = NULL;
            
            SELECT @ExistingTemplateId = [Id]
            FROM [Goals].[Templates]
            WHERE [Code] = @Code 
              AND [Companyid] = @CompanyId 
              AND [isDeleted] = 0
              AND (@UUID IS NULL OR [UUID] != @UUID);
            
            IF @ExistingTemplateId IS NOT NULL
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'Template code already exists in this company' [Message]
                RETURN 0;
            END
        END

        IF ISNULL(@UUID,'') = ''
        BEGIN
            -- Insert new record
            SET @NewUUID = NEWID();
        
            INSERT INTO [Goals].[Templates] ([UUID], [Companyid], [UsersIdCreated], [Name], [Code])
            VALUES (@NewUUID, @CompanyId, @UsersId, @Name, @Code);
        
            SET @TemplateId = SCOPE_IDENTITY();

            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                  @UsersId, 
                                                  'Templates', 
                                                  @TemplateId, 
                                                  NULL, 
                                                  @AuditLogsid OUTPUT
        
            SELECT [UUID]
            , 1 [isValid]
            , 'Template created successfully' as Message
            FROM [Goals].[Templates]
            WHERE [Id] = @TemplateId; 
        END
        ELSE
        BEGIN
            -- Capture current values for comparison
            DECLARE @OldName NVARCHAR(500) = NULL
            , @OldCode NVARCHAR(500) = NULL

            SELECT @TemplateId = [Id]
            , @OldName = [Name]
            , @OldCode = [Code]
            , @isUpdate = CASE WHEN [Name] != @Name OR
                                    ISNULL([Code], '') != ISNULL(@Code, '') THEN 1 ELSE 0 END
            FROM [Goals].[Templates]
            WHERE [UUID] = @UUID 
              AND [Companyid] = @CompanyId
              AND [isDeleted] = 0;

            IF @TemplateId IS NULL
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'Template not found or access denied' [Message]
                RETURN 0;
            END

            IF @isUpdate = 1
            BEGIN
                -- Update the record
                UPDATE [Goals].[Templates]
                SET [Name] = @Name,
                    [Code] = @Code
                WHERE [UUID] = @UUID 
                  AND [Companyid] = @CompanyId
                  AND [isDeleted] = 0;

                -- Log the changes
                EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                      @UsersId, 
                                                      'Templates', 
                                                      @TemplateId, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT

                -- Save only the values that have changed
                IF @OldName != @Name
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Name', @OldName

                IF ISNULL(@OldCode, '') != ISNULL(@Code, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Code', @OldCode
            END
        
            SELECT @UUID [UUID], 1 as [isValid], 'Template updated successfully' [Message];
        END
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END

