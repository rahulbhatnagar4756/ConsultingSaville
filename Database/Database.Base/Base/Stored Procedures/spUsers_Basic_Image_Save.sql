CREATE PROCEDURE [Base].[spUsers_Basic_Image_Save]
    @CompanyUUID        NVARCHAR(200),
    @UsersUUIDLoggedIn  NVARCHAR(200),
    @UUID               NVARCHAR(200), 
    @ImageFileLocation  NVARCHAR(MAX),
    @isSuccess          BIT OUTPUT,
    @Message            NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersidLoggedIn INT
    , @Usersid INT
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 10
    , @AuditLogsid BIGINT
    , @RootPath NVARCHAR(400)
    , @ImagePath NVARCHAR(500)
    , @DirectoryPath NVARCHAR(500)
    , @OldIsImage BIT = 0

    -- Initialize outputs
    SET @isSuccess = 0;
    SET @Message = '';

    SELECT @CompanyId = [recordID], @RootPath = [RootPath]
    FROM [Base].[dbo].[Companies] 
    WHERE [UUID] = @CompanyUUID;
    
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SET @Message = 'Valid company not supplied';
        RETURN 0;
    END

    SELECT TOP(1) @UsersidLoggedIn = [recordid]
    FROM [Base].[dbo].[users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

    IF @isHasAccess = 1
    BEGIN
        -- Validate UUID parameter
        IF @UUID IS NULL OR @UUID = ''
        BEGIN
            SET @Message = 'User UUID is required';
            RETURN 0;
        END

        -- Validate image data
        --IF @ImageBytes IS NULL OR DATALENGTH(@ImageBytes) = 0
        --BEGIN
        --    SET @Message = 'Image data is required';
        --    RETURN 0;
        --END

        -- Validate image size (max 5MB)
        --IF DATALENGTH(@ImageBytes) > 5242880
        --BEGIN
        --    SET @Message = 'Image size exceeds maximum limit of 5MB';
        --    RETURN 0;
        --END

        -- Get user record and current image status
        SELECT @Usersid = [recordid]
        , @OldIsImage = ISNULL([isImage], 0)
        FROM [Base].[dbo].[users]
        WHERE [UUID] = @UUID;

        IF @Usersid IS NULL
        BEGIN
            SET @Message = 'User not found';
            RETURN 0;
        END

        -- Construct image paths
        SET @DirectoryPath = @RootPath + '\Images\Avatars\';
        SET @ImageFileLocation = @DirectoryPath + CONVERT(NVARCHAR(20), @Usersid) + 'medium.jpg';

        BEGIN TRY

            -- Update user record to mark image as available
            UPDATE [Base].[dbo].[users]
            SET [isImage] = 1
            WHERE [recordid] = @Usersid;

            -- Log the image update
            EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                  @UsersidLoggedIn, 
                                                  'users', 
                                                  @Usersid, 
                                                  NULL, 
                                                  @AuditLogsid OUTPUT;

            -- Save audit detail for image change
            IF @OldIsImage != 1
                EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'isImage', @OldIsImage;

            SET @isSuccess = 1;
            SET @Message = 'User image saved successfully';

        END TRY
        BEGIN CATCH
            SET @isSuccess = 0;
            SET @Message = 'An error occurred while saving image: ' + ERROR_MESSAGE();
        END CATCH
    END
    ELSE
    BEGIN
        SET @isSuccess = 0;
        SET @Message = 'Access denied';
        RETURN 0;
    END
END