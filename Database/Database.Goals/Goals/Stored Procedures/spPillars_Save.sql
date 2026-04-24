CREATE PROCEDURE [Goals].[spPillars_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200) = NULL,
    @Name NVARCHAR(200),
    @Description NVARCHAR(MAX) = NULL,
    @Icons NVARCHAR(255) = 'account_balance',
    @IconColor NVARCHAR(30) = N'#362f21',
    @OrderVal INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @Usersid INT
    , @PillarId INT
    , @NewUUID NVARCHAR(200)
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 7
    , @AuditLogsid BIGINT
    , @IconsId int = NULL
    , @isUpdate bit = 0

    SELECT @CompanyId = [recordID] 
    FROM [Base].[dbo].[Companies] 
    WHERE [UUID] = @CompanyUUID;
    
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message]
        RETURN 0;
    END

    SELECT TOP(1) @Usersid = [Id]
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

    IF @isHasAccess = 1
    BEGIN

        --Huntdown the icon id
        SELECT TOP(1) @IconsId = i.[Id]
        FROM [admin].[Icon] i
        WHERE i.[Name] = @Icons

        IF @OrderVal = 0
            SELECT @OrderVal = ISNULL(MAX(p.[OrderVal]), 0) + 1
            FROM [Goals].[Pillars] p
            WHERE p.[Companyid] = @CompanyId 

        IF @UUID IS NULL OR @UUID = ''
        BEGIN
            -- Insert new record
            SET @NewUUID = NEWID();
        
            INSERT INTO [Goals].[Pillars] ([UUID]
                                         , [Companyid]
                                         , [Name]
                                         , [Description]
                                         , [IconsId]
                                         , [IconColor]
                                         , [OrderVal])
            VALUES (@NewUUID
                  , @CompanyId
                  , @Name
                  , @Description
                  , @IconsId
                  , @IconColor
                  , @OrderVal);
        
            SET @PillarId = SCOPE_IDENTITY();

            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                  @Usersid, 
                                                  'Pillars', 
                                                  @PillarId, 
                                                  NULL, 
                                                  @AuditLogsid OUTPUT
        
            SELECT [UUID], 1 [isValid], 'Pillar created successfully' as Message
            FROM [Goals].[Pillars] WHERE [Id] = @PillarId; 
        END
        ELSE
        BEGIN
            -- Capture current values for comparison
            DECLARE @OldName NVARCHAR(200) = NULL
            , @OldDescription NVARCHAR(MAX) = NULL
            , @OldIconsId INT = NULL
            , @OldIconColor NVARCHAR(30) = NULL
            , @OldOrderVal INT = NULL

            SELECT @PillarId = [Id]
            , @OldName =  [Name]
            , @OldDescription = [Description]
            , @OldIconsId = [IconsId]
            , @OldIconColor = [IconColor]
            , @OldOrderVal = [OrderVal]
            , @isUpdate = CASE WHEN [Name] != @Name OR
                                    ISNULL([Description], '') != ISNULL(@Description, '') OR
                                    ISNULL([IconsId], 0) != ISNULL(@IconsId, 0) OR
                                    [IconColor] != @IconColor OR
                                    [OrderVal] != @OrderVal THEN 1 ELSE 0 END
            FROM [Goals].[Pillars]
            WHERE [UUID] = @UUID 
              AND [Companyid] = @CompanyId
              AND [isDeleted] = 0;

            

            -- Remove trailing comma and close JSON
            IF @isUpdate = 1
            BEGIN
                
                
                -- Update the record
                UPDATE [Goals].[Pillars]
                SET [Name] = @Name,
                    [Description] = @Description,
                    [IconsId] = @IconsId,
                    [IconColor] = @IconColor,
                    [OrderVal] = @OrderVal
                WHERE [UUID] = @UUID 
                  AND [Companyid] = @CompanyId
                  AND [isDeleted] = 0;

                -- Log the changes
                EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                      @Usersid, 
                                                      'Pillars', 
                                                      @PillarId, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT

                --save adit as update                                         
                IF @OldName != @Name
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Name', @OldName

                --save only the vales that have changed
                IF ISNULL(@OldDescription, '') != ISNULL(@Description, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Description', @OldDescription

                IF ISNULL(@OldIconsId, 0) != ISNULL(@IconsId, 0)
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'IconsId', @OldIconsId

                IF @OldIconColor != @IconColor
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'IconColor', @OldIconColor

                IF @OldOrderVal != @OrderVal
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'OrderVal', @OldOrderVal

            END
        
            SELECT @UUID [UUID], 1 as [isValid], 'Pillar updated successfully' [Message];
        END
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END
