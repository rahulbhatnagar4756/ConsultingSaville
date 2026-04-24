CREATE PROCEDURE [Goals].[spUserTemplate_Save]
    @TemplatesUUID NVARCHAR(200),
    @UsersUUID NVARCHAR(200),
    @ContractPeriodsUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT;
    DECLARE @UserName NVARCHAR(200);
    DECLARE @TemplateId INT;
    DECLARE @ContractPeriodId INT;

    -- Get Logged-in User ID + Name (handles NULL middle name)
    SELECT 
        @UserId = [Id],
        @UserName = CONCAT([FirstName], ' ', ISNULL([MiddleName], ''))
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUID;

    IF @UserId IS NULL
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'Valid logged-in user not supplied' AS [Message];
        RETURN 0;
    END

    -- Validate Template ID
    SELECT @TemplateId = Id 
    FROM [Goals].[Templates]
    WHERE UUID = @TemplatesUUID 
      AND isDeleted = 0;

    IF @TemplateId IS NULL
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'Invalid Template.' AS [Message];
        RETURN 0;
    END

    -- Validate Contract Period ID
    SELECT @ContractPeriodId = Id 
    FROM [Goals].[ContractPeriods]
    WHERE UUID = @ContractPeriodsUUID 
      AND isDeleted = 0;

    IF @ContractPeriodId IS NULL
    BEGIN
        SELECT NULL AS [UUID], 0 AS [isValid], 'Invalid Contract Period.' AS [Message];
        RETURN 0;
    END

    -- Check for duplicate record
    IF EXISTS (
        SELECT 1
        FROM [Goals].[UserTemplates]
        WHERE UsersId = @UserId
          AND TemplatesId = @TemplateId
          AND ContractPeriodsId = @ContractPeriodId
    )
    BEGIN
        SELECT 
            @TemplateId AS [TemplateId], 
            @UserId AS [UserId],
            @ContractPeriodId AS [ContractPeriodId],
            0 AS [isValid],
            'This template is already assigned to this user "' +@UserName+ '" for the selected contract period.' AS [Message];
        RETURN 0;
    END

    -- Insert into UserTemplates
    INSERT INTO [Goals].[UserTemplates]
           ([TemplatesId], [UsersId], [ContractPeriodsId], [DateCreated], [isDeleted])
    VALUES
           (@TemplateId, @UserId, @ContractPeriodId, GETDATE(), 0);

    -- Return success message
    SELECT 
        @TemplateId AS [TemplateId], 
        @UserId AS [UserId], 
        @ContractPeriodId AS [ContractPeriodId],
        1 AS [isValid], 
        'User Template saved successfully.' AS [Message];
END