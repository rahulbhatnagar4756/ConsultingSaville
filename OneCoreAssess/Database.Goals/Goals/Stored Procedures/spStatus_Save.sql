-- Status - Save (Insert/Update)
CREATE PROCEDURE [Goals].[spStatus_Save]
    @UUID NVARCHAR(200) = NULL,
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @Name NVARCHAR(100),
    @Description NVARCHAR(500) = NULL,
    @OrderVal INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @StatusId INT
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
            SELECT @OrderVal = ISNULL(MAX(s.[OrderVal]), 0) + 1
            FROM [Goals].[Status] s
            WHERE s.[Companyid] = @CompanyId 
              AND s.[isDeleted] = 0

        IF @UUID IS NULL OR @UUID = ''
        BEGIN
            -- Insert new record
            SET @NewUUID = NEWID();
        
            INSERT INTO [Goals].[Status] ([UUID], [Companyid], [Name], [Description], [OrderVal])
            VALUES (@NewUUID, @CompanyId, @Name, @Description, @OrderVal);
        
            SET @StatusId = SCOPE_IDENTITY();

            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                  @UsersId, 
                                                  'Status', 
                                                  @StatusId, 
                                                  NULL, 
                                                  @AuditLogsid OUTPUT
        
            SELECT [UUID]
            , 1 [isValid]
            , 'Status created successfully' as Message
            FROM [Goals].[Status]
            WHERE [Id] = @StatusId; 
        END
        ELSE
        BEGIN
            -- Capture current values for comparison
            DECLARE @OldName NVARCHAR(100) = NULL
            , @OldDescription NVARCHAR(500) = NULL
             

            SELECT @StatusId = [Id]
            , @OldName = [Name]
            , @OldDescription = [Description] 
            , @isUpdate = CASE WHEN [Name] != @Name OR
                                    ISNULL([Description], '') != ISNULL(@Description, '') OR
                                    [OrderVal] != @OrderVal THEN 1 ELSE 0 END
            FROM [Goals].[Status]
            WHERE [UUID] = @UUID 
              AND [Companyid] = @CompanyId
              AND [isDeleted] = 0;

            IF @isUpdate = 1
            BEGIN
                -- Update the record
                UPDATE [Goals].[Status]
                SET [Name] = @Name,
                    [Description] = @Description,
                    [OrderVal] = @OrderVal
                WHERE [UUID] = @UUID 
                  AND [Companyid] = @CompanyId
                  AND [isDeleted] = 0;

                -- Log the changes
                EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                      @UsersId, 
                                                      'Status', 
                                                      @StatusId, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT

                -- Save only the values that have changed
                IF @OldName != @Name
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Name', @OldName

                IF ISNULL(@OldDescription, '') != ISNULL(@Description, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Description', @OldDescription
             
            END
        
            SELECT @UUID [UUID], 1 as [isValid], 'Status updated successfully' [Message];
        END
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END
