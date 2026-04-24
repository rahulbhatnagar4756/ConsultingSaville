USE [Goals]
GO
/****** Object:  StoredProcedure [admin].[spCommentAttachments_Delete]    Script Date: 26/11/2025 12:22:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [admin].[spCommentAttachments_Delete]
(
    @CompanyUUID        NVARCHAR(200),
    @UsersUUIDLoggedIn  NVARCHAR(200),
    @UUID               NVARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE
        @Companyid BIGINT,
        @Usersid BIGINT,
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
    -- ACCESS CHECK
    ---------------------------------------------------------
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access]
            @UsersUUIDLoggedIn,
            @CompanyUUID,
            @SecurityRoleAccessidEdit;

    
    IF @isHasAccess = 0
    BEGIN
        SET @Message = 'Access denied: You do not have permission to delete attachments';
        GOTO END_PROC;
    END
    

    ---------------------------------------------------------
    -- Validate attachment exists
    ---------------------------------------------------------
    SELECT @AttachmentId = Id
    FROM [admin].[CommentAttachments]
    WHERE UUID = @UUID AND isDeleted = 0;

    IF @AttachmentId IS NULL
    BEGIN
        SET @Message = 'Attachment not found';
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- Soft Delete
    ---------------------------------------------------------
    UPDATE [admin].[CommentAttachments]
    SET isDeleted = 1
    WHERE UUID = @UUID;

    EXEC [audit].[spAuditLogs_Log_Delete]
        @Companyid, @Usersid,
        'CommentAttachments', @AttachmentId,
        NULL, @AuditLogsid OUTPUT;

    SET @isValid = 1;
    SET @Message = 'Attachment deleted successfully';

END_PROC:

    SELECT 
        @UUID AS UUID,
        @isValid AS isValid,
        @Message AS Message;

END
