USE [Goals]
GO
/****** Object:  StoredProcedure [admin].[spComments_Delete]    Script Date: 26/11/2025 12:22:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [admin].[spComments_Delete]
    @CompanyUUID        NVARCHAR(200),
    @UsersUUIDLoggedIn  NVARCHAR(200),
    @UUID               NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @Companyid BIGINT,
        @Usersid BIGINT,
        @Commentsid BIGINT,
        @isHasAccess BIT = 1,
        @SecurityRoleAccessidDelete INT = 11,
        @AuditLogsid BIGINT,
        @isValid BIT = 0,
        @Message NVARCHAR(MAX) = '';

    ---------------------------------------------------------
    -- Validate UUID input
    ---------------------------------------------------------
    IF LTRIM(RTRIM(ISNULL(@UUID, ''))) = ''
    BEGIN
        SET @Message = 'Comment UUID is required';
        SET @isValid = 0;
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- Validate Company
    ---------------------------------------------------------
    SELECT @Companyid = [recordID] 
    FROM [Base].[Companies] 
    WHERE [UUID] = @CompanyUUID;

    IF ISNULL(@Companyid, 0) = 0
    BEGIN
        SET @Message = 'Valid company not supplied';
        SET @isValid = 0;
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- Validate User
    ---------------------------------------------------------
    SELECT TOP(1) @Usersid = [id]
    FROM [Base].[users] 
    WHERE [UUID] = @UsersUUIDLoggedIn;

    IF ISNULL(@Usersid, 0) = 0
    BEGIN
        SET @Message = 'Valid user not supplied';
        SET @isValid = 0;
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- Get Comment ID
    ---------------------------------------------------------
    SELECT @Commentsid = [Id]
    FROM [admin].[Comments]
    WHERE [UUID] = @UUID AND [isDeleted] = 0;

    IF @Commentsid IS NULL
    BEGIN
        SET @Message = 'Comment not found or already deleted';
        SET @isValid = 0;
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- ACCESS CHECK
    ---------------------------------------------------------
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access]
            @UsersUUIDLoggedIn,
            @CompanyUUID,
            @SecurityRoleAccessidDelete;

    ---- If no role permission, allow delete only if the user is the creator
    IF @isHasAccess = 0
    BEGIN
        IF EXISTS (
            SELECT 1 FROM [admin].[Comments]
            WHERE [Id] = @Commentsid AND [Usersid] = @Usersid
        )
        BEGIN
            SET @isHasAccess = 1;
        END
    END

    IF @isHasAccess = 0
    BEGIN
        SET @Message = 'Access denied. You can only delete your own comments or need delete permissions';
        SET @isValid = 0;
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- SOFT DELETE COMMENT
    ---------------------------------------------------------
    UPDATE [admin].[Comments]
    SET [isDeleted] = 1
    WHERE [Id] = @Commentsid;

    ---------------------------------------------------------
    -- SOFT DELETE ATTACHMENTS
    ---------------------------------------------------------
    UPDATE [admin].[CommentAttachments]
    SET [isDeleted] = 1
    WHERE [CommentsId] = @Commentsid AND [isDeleted] = 0;

    ---------------------------------------------------------
    -- AUDIT LOG
    ---------------------------------------------------------
    EXEC [audit].[spAuditLogs_Log_Delete]
         @Companyid,
         @Usersid,
         'Comments',
         @Commentsid,
         NULL,
         @AuditLogsid OUTPUT;

    SET @isValid = 1;
    SET @Message = 'Comment deleted successfully';


END_PROC:

    ---------------------------------------------------------
    -- FINAL SELECT (ALWAYS RETURNS)
    ---------------------------------------------------------
    SELECT
        @UUID AS UUID,
        @isValid AS isValid,
        @Message AS Message;

END
