CREATE PROCEDURE [Goals].[spToleranceRanges_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200) = NULL,
    @ToleranceSetsUUID NVARCHAR(200),
    @RangeStart DECIMAL(18,2) = NULL,
    @Score DECIMAL(18,2) = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @ToleranceRangeId INT
    , @ToleranceSetsId INT
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

    -- Get ToleranceSet ID and verify company ownership
    SELECT @ToleranceSetsId = [Id]
    FROM [Goals].[ToleranceSets]
    WHERE [UUID] = @ToleranceSetsUUID 
      AND [Companyid] = @CompanyId
      AND [isDeleted] = 0;

    IF ISNULL(@ToleranceSetsId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Invalid tolerance set or access denied' [Message]
        RETURN 0;
    END

    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

    IF @isHasAccess = 1
    BEGIN

        IF @Score > 0 AND (LEN(ISNULL(@UUID,'')) = 0 OR ISNULL(@UUID,'') = '')  
        BEGIN
            DECLARE @isCreateEmptyZero bit = 0

            SELECT @isCreateEmptyZero = CASE WHEN COUNT(*) = 0 THEN 1 ELSE 0 END
            FROM [Goals].[ToleranceRanges] r
            WHERE r.[ToleranceSetsid] = @ToleranceSetsId
                AND r.[isDeleted] = 0
                AND r.[Score] <= 0 
                
            IF @isCreateEmptyZero = 1
                EXEC [Goals].[spToleranceRanges_Save] @CompanyUUID ,
                                                      @UsersUUIDLoggedIn,
                                                      NULL ,
                                                      @ToleranceSetsUUID,
                                                      NULL,
                                                      0;

        END



        IF @UUID IS NULL OR @UUID = ''
        BEGIN
            -- Insert new record
            SET @NewUUID = NEWID();
        
            INSERT INTO [Goals].[ToleranceRanges] ([UUID], [ToleranceSetsid], [RangeStart], [Score])
            VALUES (@NewUUID, @ToleranceSetsId, @RangeStart, @Score);
        
            SET @ToleranceRangeId = SCOPE_IDENTITY();

            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                  @UsersId, 
                                                  'ToleranceRanges', 
                                                  @ToleranceRangeId, 
                                                  NULL, 
                                                  @AuditLogsid OUTPUT
        
            SELECT [UUID]
            , 1 [isValid]
            , 'Tolerance range created successfully' as Message
            FROM [Goals].[ToleranceRanges]
            WHERE [Id] = @ToleranceRangeId; 
        END
        ELSE
        BEGIN
            -- Capture current values for comparison
            DECLARE @OldRangeStart DECIMAL(18,2) = NULL
            , @OldScore DECIMAL(18,2) = NULL

            -- Verify the tolerance range belongs to the company before updating
            IF NOT EXISTS (
                SELECT 1 
                FROM [Goals].[ToleranceRanges] tr
                INNER JOIN [Goals].[ToleranceSets] ts ON tr.[ToleranceSetsid] = ts.[Id]
                WHERE tr.[UUID] = @UUID 
                  AND ts.[Companyid] = @CompanyId
                  AND tr.[isDeleted] = 0
            )
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'Tolerance range not found or access denied' [Message]
                RETURN 0;
            END

            -- Get current values and check for changes
            SELECT @ToleranceRangeId = [Id]
            , @OldRangeStart = [RangeStart]
            , @OldScore = [Score]
            , @isUpdate = CASE WHEN ISNULL([RangeStart], 0) != ISNULL(@RangeStart, 0) OR
                                    [Score] != @Score THEN 1 ELSE 0 END
            FROM [Goals].[ToleranceRanges]
            WHERE [UUID] = @UUID;

            IF @isUpdate = 1
            BEGIN
                -- Update existing record
                UPDATE [Goals].[ToleranceRanges]
                SET [RangeStart] = @RangeStart,
                    [Score] = @Score
                WHERE [UUID] = @UUID;

                -- Log the changes
                EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                      @UsersId, 
                                                      'ToleranceRanges', 
                                                      @ToleranceRangeId, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT

                -- Save only the values that have changed
                IF ISNULL(@OldRangeStart, 0) != ISNULL(@RangeStart, 0)
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'RangeStart', @OldRangeStart

                IF @OldScore != @Score
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Score', @OldScore
            END
        
            SELECT @UUID [UUID]
            , 1 as [isValid]
            , 'Tolerance range updated successfully' [Message];
        END
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message]
        RETURN 0;
    END
END
