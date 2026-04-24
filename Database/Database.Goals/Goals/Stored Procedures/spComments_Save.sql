USE [Goals]
GO
/****** Object:  StoredProcedure [admin].[spComments_Save]    Script Date: 26/11/2025 12:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [admin].[spComments_Save]
    @CompanyUUID        NVARCHAR(200),
    @UsersUUIDLoggedIn  NVARCHAR(200),
    @UUID               NVARCHAR(200) = NULL,
    @TableNameid        INT,
    @TableTargetUUID    NVARCHAR(200),   
    @Comment            NVARCHAR(MAX),
    @CommentTypesid     INT = 1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @Companyid BIGINT,
        @Usersid BIGINT,
        @Commentsid BIGINT,
        @isHasAccess BIT = 1,
        @SecurityRoleAccessidEdit INT = 10,
        @AuditLogsid BIGINT,
        @isUpdate BIT = 0,
        @isValid BIT = 0,
        @Message NVARCHAR(MAX) = '',
        @TableTargetid BIGINT;  

    ---------------------------------------------------------
    -- Validate Company
    ---------------------------------------------------------
    SELECT @Companyid = [recordID] 
    FROM [Base].[Companies] 
    WHERE [UUID] = @CompanyUUID;

    IF ISNULL(@Companyid, 0) = 0
    BEGIN
        SET @UUID = NULL;
        SET @isValid = 0;
        SET @Message = 'Valid company not supplied';
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
        SET @UUID = NULL;
        SET @isValid = 0;
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

    -- Access check block restored    
    IF @isHasAccess = 0
    BEGIN
        SET @UUID = NULL;
        SET @isValid = 0;
        SET @Message = 'Access denied: You do not have permission to add or edit comments';
        GOTO END_PROC;
    END
    

    ---------------------------------------------------------
    -- Validate Required Fields
    ---------------------------------------------------------
    IF LTRIM(RTRIM(ISNULL(@Comment,''))) = ''
    BEGIN
        SET @UUID = NULL; 
        SET @isValid = 0;
        SET @Message = 'Comment text is required';
        GOTO END_PROC;
    END

    IF ISNULL(@TableNameid, 0) = 0
    BEGIN
        SET @UUID = NULL; 
        SET @isValid = 0;
        SET @Message = 'Table name ID is required';
        GOTO END_PROC;
    END

     ---------------------------------------------------------
-- Convert TableTargetUUID → TableTargetId (KPI or KPA)
---------------------------------------------------------

-- 1) First try to find in KPI
SELECT @TableTargetId = [Id]
FROM [Goals].[Goals].[KPI]
WHERE [UUID] = @TableTargetUUID;

-- 2) If not found in KPI, try KPA
IF ISNULL(@TableTargetId, 0) = 0
BEGIN
    SELECT @TableTargetId = [Id]
    FROM [Goals].[Goals].[KPA]
    WHERE [UUID] = @TableTargetUUID;
END


-- 3) If still not found → return error
IF ISNULL(@TableTargetId, 0) = 0
BEGIN
    SET @UUID = NULL;
    SET @isValid = 0;
    SET @Message = 'Invalid Table Target UUID: Not found in KPI/KPA';
    GOTO END_PROC;
END


    ---------------------------------------------------------
    -- Validate Comment Type
    ---------------------------------------------------------
    IF NOT EXISTS (
        SELECT 1 FROM [admin].[CommentTypes] 
        WHERE [Id] = @CommentTypesid AND [isDeleted] = 0
    )
    BEGIN
        SET @UUID = NULL; 
        SET @isValid = 0;
        SET @Message = 'Invalid comment type';
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- Validate Table Name
    ---------------------------------------------------------
    IF NOT EXISTS (
        SELECT 1 FROM [Goals].[TableName] WHERE [Id] = @TableNameid
    )
    BEGIN
        SET @UUID = NULL;
        SET @isValid = 0;
        SET @Message = 'Invalid table name ID';
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- INSERT NEW COMMENT
    ---------------------------------------------------------
    IF @UUID IS NULL OR @UUID = ''
    BEGIN
        SET @UUID = NEWID();

        INSERT INTO [admin].[Comments]
        (
            [UUID], [Companyid], [Usersid],
            [TableNameid], [TableTargetid],
            [Comment], [CommentTypesid]
        )
        VALUES
        (
            @UUID, @Companyid, @Usersid,
            @TableNameid, @TableTargetid,
            @Comment, @CommentTypesid
        );

        SET @Commentsid = SCOPE_IDENTITY();

        EXEC [audit].[spAuditLogs_Log_Insert]
            @Companyid, @Usersid, 
            'Comments', @Commentsid, 
            NULL, @AuditLogsid OUTPUT;

        SET @isValid = 1;
        SET @Message = 'Comment created successfully';
        GOTO END_PROC;
    END

    ---------------------------------------------------------
    -- UPDATE EXISTING COMMENT
    ---------------------------------------------------------
    SELECT 
        @Commentsid = [Id],
        @isUpdate = CASE 
                        WHEN [Comment] != @Comment 
                          OR [CommentTypesid] != @CommentTypesid 
                        THEN 1 ELSE 0 
                    END
    FROM [admin].[Comments]
    WHERE [UUID] = @UUID AND [isDeleted] = 0;

    IF @Commentsid IS NULL
    BEGIN
        SET @UUID = NULL;
        SET @isValid = 0;
        SET @Message = 'Comment not found';
        GOTO END_PROC;
    END

    -- Only creator can edit
    IF NOT EXISTS (SELECT 1 FROM [admin].[Comments]
                   WHERE [Id] = @Commentsid AND [Usersid] = @Usersid)
    BEGIN
        SET @UUID = NULL;
        SET @isValid = 0;
        SET @Message = 'You can only edit your own comments';
        GOTO END_PROC;
    END

    IF @isUpdate = 1
    BEGIN
        UPDATE [admin].[Comments]
        SET 
            [Comment] = @Comment,
            [CommentTypesid] = @CommentTypesid
        WHERE [UUID] = @UUID;

        EXEC [audit].[spAuditLogs_Log_Update]
            @Companyid, @Usersid, 
            'Comments', @Commentsid, 
            NULL, @AuditLogsid OUTPUT;
    END

    SET @isValid = 1;
    SET @Message = 'Comment updated successfully';

END_PROC:

    SELECT 
        @UUID AS UUID,
        @isValid AS isValid,
        @Message AS Message;

END
