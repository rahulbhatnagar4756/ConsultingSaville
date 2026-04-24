CREATE PROCEDURE [Goals].[spToleranceSets_Save]
    @UUID NVARCHAR(200) = NULL,
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @Name NVARCHAR(200),
    @Description NVARCHAR(MAX) = NULL,
    @MaxTotal FLOAT = 100,
    @OrderVal INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @ToleranceSetId INT
    , @NewUUID NVARCHAR(200)
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 7
    , @AuditLogsid BIGINT
    , @isUpdate BIT = 0

    SELECT @CompanyId = [recordID] 
    FROM [Base].[dbo].[Companies] 
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
        -- Auto-assign OrderVal if not provided
        IF @OrderVal = 0
            SELECT @OrderVal = ISNULL(MAX(ts.[OrderVal]), 0) + 1
            FROM [Goals].[ToleranceSets] ts
            WHERE ts.[Companyid] = @CompanyId 
              AND ts.[isDeleted] = 0

        IF @UUID IS NULL OR @UUID = ''
        BEGIN
            -- Insert new record
            SET @NewUUID = NEWID();
        
            INSERT INTO [Goals].[ToleranceSets] ([UUID], [Companyid], [Name], [Description], [MaxTotal], [OrderVal])
            VALUES (@NewUUID, @CompanyId, @Name, @Description, @MaxTotal, @OrderVal);
        
            SET @ToleranceSetId = SCOPE_IDENTITY();

            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                  @UsersId, 
                                                  'ToleranceSets', 
                                                  @ToleranceSetId, 
                                                  NULL, 
                                                  @AuditLogsid OUTPUT
        
            SELECT [UUID]
            , 1 [isValid]
            , 'Tolerance Set created successfully' as Message
            FROM [Goals].[ToleranceSets]
            WHERE [Id] = @ToleranceSetId; 
        END
        ELSE
        BEGIN
            -- Capture current values for comparison
            DECLARE @OldName NVARCHAR(200) = NULL
            , @OldDescription NVARCHAR(MAX) = NULL
            , @OldMaxTotal FLOAT = NULL
            , @OldOrderVal INT = NULL

            SELECT @ToleranceSetId = [Id]
            , @OldName = [Name]
            , @OldDescription = [Description]
            , @OldMaxTotal = [MaxTotal]
            , @OldOrderVal = [OrderVal]
            , @isUpdate = CASE WHEN [Name] != @Name OR
                                    ISNULL([Description], '') != ISNULL(@Description, '') OR
                                    [MaxTotal] != @MaxTotal OR
                                    [OrderVal] != @OrderVal THEN 1 ELSE 0 END
            FROM [Goals].[ToleranceSets]
            WHERE [UUID] = @UUID 
              AND [Companyid] = @CompanyId
              AND [isDeleted] = 0;

            IF @isUpdate = 1
            BEGIN
                -- Update the record
                UPDATE [Goals].[ToleranceSets]
                SET [Name] = @Name,
                    [Description] = @Description,
                    [MaxTotal] = @MaxTotal,
                    [OrderVal] = @OrderVal
                WHERE [UUID] = @UUID 
                  AND [Companyid] = @CompanyId
                  AND [isDeleted] = 0;

                -- Log the changes
                EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                      @UsersId, 
                                                      'ToleranceSets', 
                                                      @ToleranceSetId, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT

                -- Save only the values that have changed
                IF @OldName != @Name
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Name', @OldName

                IF ISNULL(@OldDescription, '') != ISNULL(@Description, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Description', @OldDescription

                IF @OldMaxTotal != @MaxTotal
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'MaxTotal', @OldMaxTotal

                IF @OldOrderVal != @OrderVal
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'OrderVal', @OldOrderVal
            END
        
            SELECT @UUID [UUID], 1 as [isValid], 'Tolerance Set updated successfully' [Message];
        END
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END
