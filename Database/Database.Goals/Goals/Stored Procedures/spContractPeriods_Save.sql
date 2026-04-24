CREATE PROCEDURE [Goals].[spContractPeriods_Save]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UUID NVARCHAR(200) = NULL,
    @Name NVARCHAR(200),
    @Description NVARCHAR(MAX) = NULL,
    @DateStart DATETIME2(7),
    @DateEnd DATETIME2(7),
    @Year INT,
    @DateTerminationActive DATETIME2(7) = NULL,
    @isActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CompanyId INT,
            @UsersId INT,
            @ContractPeriodId BIGINT,
            @NewUUID NVARCHAR(200),
            @isHasAccess BIT = 0,
            @SecurityRoleAccessIdEdit INT = 7,
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

   
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] 
        @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessIdEdit;

    IF @isHasAccess = 1
    BEGIN
    

        -- Validate date range
        IF @DateStart >= @DateEnd
        BEGIN
            SELECT NULL [UUID], 0 [isValid], 'Start date must be before end date' [Message];
            RETURN 0;
        END

        IF @UUID IS NULL OR @UUID = ''
        BEGIN
            -- Insert new record
            SET @NewUUID = NEWID();

            INSERT INTO [Goals].[ContractPeriods] 
            (
                [UUID],
                [Companyid],
                [Name],
                [Description],
                [DateStart],
                [DateEnd],
                [Year],
                [DateTerminationActive],
                [isActive],
                [isDeleted]
            )
            VALUES 
            (
                @NewUUID,
                @CompanyId,
                @Name,
                @Description,
                @DateStart,
                @DateEnd,
                @Year,
                @DateTerminationActive,
                @isActive,
                0
            );

            SET @ContractPeriodId = SCOPE_IDENTITY();

            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] 
                @CompanyId, @UsersId, 'ContractPeriods', @ContractPeriodId, NULL, @AuditLogsId OUTPUT;

            SELECT [UUID], 1 [isValid], 'Contract period created successfully' as [Message]
            FROM [Goals].[ContractPeriods]
            WHERE [Id] = @ContractPeriodId;
        END
        ELSE
        BEGIN
    -- Check if the record exists for the given UUID and Company
    SELECT @ContractPeriodId = [Id]
    FROM [Goals].[ContractPeriods]
    WHERE [UUID] = @UUID AND [CompanyId] = @CompanyId AND [isDeleted] = 0;

    IF ISNULL(@ContractPeriodId, 0) = 0
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Contract period not found or already deleted' [Message];
        RETURN 0;
    END

    -- Perform the update
    UPDATE [Goals].[ContractPeriods]
    SET 
        [Name] = @Name,
        [Description] = @Description,
        [DateStart] = @DateStart,
        [DateEnd] = @DateEnd,
        [Year] = @Year,
        [DateTerminationActive] = @DateTerminationActive,
        [isActive] = @isActive
    WHERE [Id] = @ContractPeriodId;

    -- Log the update
    EXEC [audit].[spAuditLogs_Log_Insert] 
        @CompanyId, @UsersId, 'ContractPeriods', @ContractPeriodId, NULL, @AuditLogsId OUTPUT;

    SELECT @UUID [UUID], 1 [isValid], 'Contract period updated successfully' [Message];
END

    END
    ELSE
    BEGIN
        SELECT NULL [UUID], 0 [isValid], 'Access denied' [Message];
        RETURN 0;
    END
    
END