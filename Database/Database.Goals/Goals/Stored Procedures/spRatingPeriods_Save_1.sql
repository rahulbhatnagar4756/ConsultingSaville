CREATE PROCEDURE [Goals].[spRatingPeriods_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200) = NULL,
    @Name NVARCHAR(200),
    @DisplayName NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT,
            @UsersId INT,
            @RatingPeriodsId BIGINT,
            @NewUUID NVARCHAR(200),
            @isHasAccess BIT = 0,
            @SecurityRoleAccessIdEdit INT = 7, -- Same security role as Pillars
            @AuditLogsId BIGINT,
            @isUpdate BIT = 0;

    -- Validate Company UUID and get Company ID
    SELECT @CompanyId = [recordID] 
    FROM [Base].[dbo].[Companies] 
    WHERE [UUID] = @CompanyUUID;

    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Valid company not supplied' [Message];
        RETURN 0;
    END

    -- Get User ID
    SELECT TOP(1) @UsersId = [Id] 
    FROM [Base].[Users] 
    WHERE [UUID] = @UsersUUIDLoggedIn;

    
    ------------------------------------------------------------------------
    -- ACCESS CHECK SECTION (Commented temporarily)
    ------------------------------------------------------------------------
    -- Check security access
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] 
        @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessIdEdit;

    IF @isHasAccess = 1
    BEGIN
    ------------------------------------------------------------------------
    

        -- Validate required fields
        IF LTRIM(RTRIM(ISNULL(@Name, ''))) = ''
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Name is required' [Message];
            RETURN 0;
        END

        IF LTRIM(RTRIM(ISNULL(@DisplayName, ''))) = ''
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Display Name is required' [Message];
            RETURN 0;
        END

        IF @UUID IS NULL OR @UUID = ''
        BEGIN
            -- Insert new record
            SET @NewUUID = NEWID();

            INSERT INTO [Goals].[RatingPeriods] 
            (
                [UUID],
                [Companyid],
                [Name],
                [DisplayName] 
            )
            VALUES 
            (
                @NewUUID,
                @CompanyId,
                @Name,
                @DisplayName 
            );

            SET @RatingPeriodsId = SCOPE_IDENTITY();

            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] 
                @CompanyId, @UsersId, 'RatingPeriods', @RatingPeriodsId, NULL, @AuditLogsId OUTPUT;

            SELECT [UUID], 1 [isValid], 'Rating period created successfully' as [Message]
            FROM [Goals].[RatingPeriods]
            WHERE [Id] = @RatingPeriodsId;
        END
        ELSE
        BEGIN
            -- Check if the record exists for the given UUID and Company
            SELECT @RatingPeriodsId = [Id]
            FROM [Goals].[RatingPeriods]
            WHERE [UUID] = @UUID 
                AND [CompanyId] = @CompanyId 
                AND [isDeleted] = 0;

            IF ISNULL(@RatingPeriodsId, 0) = 0
            BEGIN
                SELECT NULL [UUID], 0 [isValid], 'Rating period not found or already deleted' [Message];
                RETURN 0;
            END

            -- Perform the update
            UPDATE [Goals].[RatingPeriods]
            SET 
                [Name] = @Name,
                [DisplayName] = @DisplayName
            WHERE [Id] = @RatingPeriodsId;

            -- Log the update
            EXEC [audit].[spAuditLogs_Log_Insert] 
                @CompanyId, @UsersId, 'RatingPeriods', @RatingPeriodsId, NULL, @AuditLogsId OUTPUT;

            SELECT @UUID [UUID], 1 [isValid], 'Rating period updated successfully' [Message];
        END

    
    ------------------------------------------------------------------------
    -- END OF ACCESS CHECK SECTION (Commented temporarily)
    ------------------------------------------------------------------------
    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message];
        RETURN 0;
    END
    ------------------------------------------------------------------------
    
END