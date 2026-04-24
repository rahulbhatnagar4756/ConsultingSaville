
CREATE PROCEDURE [admin].[spCommentAttachments_Upsert]
(
    @CompanyUUID        NVARCHAR(200),
    @UsersUUIDLoggedIn  NVARCHAR(200),
    @UUID               NVARCHAR(200) = NULL,
    @CommentsUUID       NVARCHAR(200),
    @FileName           NVARCHAR(500),
    @FilePath           NVARCHAR(1000),
    @FileSize           BIGINT,
    @MimeType           NVARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @Companyid BIGINT,
        @Usersid BIGINT,
        @CommentsId BIGINT,
        @AttachmentId BIGINT,
        @isValid BIT = 0,
        @Message NVARCHAR(MAX) = '',
        @AuditLogsid BIGINT,
        @isHasAccess BIT = 1,
        @SecurityRoleAccessidEdit INT = 10;

    ---------------------------------------------------------
    -- Validate Company
    ---------------------------------------------------------
    SELECT @Companyid = recordID 
    FROM [Base].[Companies] 
    WHERE UUID = @CompanyUUID;

    IF ISNULL(@Companyid, 0) = 0
    BEGIN
        SET @Message = 'Valid company not supplied';
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- Validate User
    ---------------------------------------------------------
    SELECT @Usersid = id
    FROM [Base].[users]
    WHERE UUID = @UsersUUIDLoggedIn;

    IF ISNULL(@Usersid, 0) = 0
    BEGIN
        SET @Message = 'Valid user not supplied';
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- ACCESS CHECK (same logic as Comments Save SP)
    ---------------------------------------------------------
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access]
            @UsersUUIDLoggedIn,
            @CompanyUUID,
            @SecurityRoleAccessidEdit;

    /*
    IF @isHasAccess = 0
    BEGIN
        SET @Message = 'Access denied: You do not have permission to upload or edit attachments';
        GOTO END_PROC;
    END
    */

    ---------------------------------------------------------
    -- Validate Comment UUID → CommentId
    ---------------------------------------------------------
    SELECT @CommentsId = Id 
    FROM [admin].[Comments]
    WHERE UUID = @CommentsUUID AND isDeleted = 0;

    IF ISNULL(@CommentsId, 0) = 0
    BEGIN
        SET @Message = 'Invalid Comments UUID';
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- UPSERT
    ---------------------------------------------------------
    IF @UUID IS NULL OR @UUID = ''
    BEGIN
        SET @UUID = NEWID();

        INSERT INTO [admin].[CommentAttachments]
        (
            UUID, CommentsId, FileName, FilePath, FileSize, MimeType
        )
        VALUES
        (
            @UUID, @CommentsId, @FileName, @FilePath, @FileSize, @MimeType
        );

        SET @AttachmentId = SCOPE_IDENTITY();

        EXEC [audit].[spAuditLogs_Log_Insert]
            @Companyid, @Usersid,
            'CommentAttachments', @AttachmentId,
            NULL, @AuditLogsid OUTPUT;

        SET @isValid = 1;
        SET @Message = 'Attachment created successfully';
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- UPDATE
    ---------------------------------------------------------
    SELECT @AttachmentId = Id 
    FROM [admin].[CommentAttachments]
    WHERE UUID = @UUID AND isDeleted = 0;

    IF @AttachmentId IS NULL
    BEGIN
        SET @UUID = NULL;
        SET @Message = 'Attachment not found';
        GOTO END_PROC;
    END

    UPDATE [admin].[CommentAttachments]
    SET 
        FileName = @FileName,
        FilePath = @FilePath,
        FileSize = @FileSize,
        MimeType = @MimeType
    WHERE UUID = @UUID;

    EXEC [audit].[spAuditLogs_Log_Update]
        @Companyid, @Usersid,
        'CommentAttachments', @AttachmentId,
        NULL, @AuditLogsid OUTPUT;

    SET @isValid = 1;
    SET @Message = 'Attachment updated successfully';

END_PROC:

    SELECT 
        @UUID AS UUID,
        @isValid AS isValid,
        @Message AS Message;

END