CREATE PROCEDURE [Base].[spUsers_Basic_Save]
    @CompanyUUID        NVARCHAR(200),
    @UsersUUIDLoggedIn  NVARCHAR(200),
    @UUID               NVARCHAR(200) = NULL OUTPUT,
    @FirstName          NVARCHAR(200),
    @LastName           NVARCHAR(200),
    @IDNumber           NVARCHAR(200),
    @Email              NVARCHAR(200),
    @Mobile             NVARCHAR(200),
    @Gender             NVARCHAR(200),
    @Ethnicity          NVARCHAR(200),
    @DateOfBirth        DATETIME,
    @isSuccess          BIT OUTPUT,
    @Message            NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersidLoggedIn INT
    , @UsersId INT 
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidEdit INT = 10
    , @AuditLogsid BIGINT
    , @isUpdate BIT = 0

    -- Initialize outputs
    SET @isSuccess = 0;
    SET @Message = '';

    SELECT @CompanyId = [recordID] 
    FROM [Base].[dbo].[Companies] 
    WHERE [UUID] = @CompanyUUID;
    
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SET @UUID = NULL;
        SET @Message = 'Valid company not supplied';
        RETURN 0;
    END

    SELECT TOP(1) @UsersidLoggedIn = [recordid]
    FROM [Base].[dbo].[users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidEdit;

    IF @isHasAccess = 1
    BEGIN
        -- Validate required fields
        IF LTRIM(RTRIM(ISNULL(@FirstName, ''))) = ''
        BEGIN
            SET @UUID = NULL;
            SET @Message = 'First name is required';
            RETURN 0;
        END

        IF LTRIM(RTRIM(ISNULL(@LastName, ''))) = ''
        BEGIN
            SET @UUID = NULL;
            SET @Message = 'Last name is required';
            RETURN 0;
        END

        IF LTRIM(RTRIM(ISNULL(@Email, ''))) = ''
        BEGIN
            SET @UUID = NULL;
            SET @Message = 'Email is required';
            RETURN 0;
        END

        IF @UUID IS NULL OR @UUID = ''
        BEGIN
            -- Check email uniqueness for new user
            --IF EXISTS (
            --    SELECT 1 
            --    FROM [Base].[dbo].[users] u
            --    WHERE u.[email] = @Email
            --)
            --BEGIN
            --    SET @UUID = NULL;
            --    SET @Message = 'Email address already exists';
            --    RETURN 0;
            --END

            -- Check ID Number uniqueness if provided
            IF @IDNumber IS NOT NULL AND @IDNumber != ''
            BEGIN
                IF EXISTS (
                    SELECT 1 FROM [Base].[dbo].[users] u
                    WHERE u.[IDNumber] = @IDNumber
                )
                BEGIN
                    SET @UUID = NULL;
                    SET @Message = 'ID Number already exists';
                    RETURN 0;
                END
            END

            --if date of birth is not provided then extract from the id number 
            IF LEN(ISNULL(@DateOfBirth,'')) = 0 AND @IDNumber IS NOT NULL  
            BEGIN
                SET @DateOfBirth = [Base].[func_Tool_SAIDNumber_ReturnBirthDate] (@IDNumber)
            END
           
        
            -- Insert new record
            INSERT INTO [Base].[dbo].[users] ( [firstname]
                                           , [lastname]
                                           , [IDNumber]
                                           , [email]
                                           , [mobile]
                                           , [Gender]
                                           , [Race]
                                           , [DateOfBirth]
                                           , [isImage]
                                           , [DateCreated] )
            VALUES ( @FirstName
                  , @LastName
                  , @IDNumber
                  , @Email
                  , @Mobile
                  , @Gender
                  , @Ethnicity
                  , @DateOfBirth
                  , 0
                  , GETUTCDATE());
        
            SET @UsersId = SCOPE_IDENTITY();

            -- Log creation
            EXEC [audit].[spAuditLogs_Log_Insert] @CompanyId, 
                                                  @UsersidLoggedIn, 
                                                  'users', 
                                                  @UsersId, 
                                                  NULL, 
                                                  @AuditLogsid OUTPUT;
        
            SET @isSuccess = 1;
            SET @Message = 'User created successfully';
            RETURN 1;
        END
        ELSE
        BEGIN
            -- Capture current values for comparison
            DECLARE @OldFirstName NVARCHAR(200) = NULL
            , @OldLastName NVARCHAR(200) = NULL
            , @OldIDNumber NVARCHAR(200) = NULL
            , @OldEmail NVARCHAR(200) = NULL
            , @OldMobile NVARCHAR(200) = NULL
            , @OldGender NVARCHAR(200) = NULL
            , @OldEthnicity NVARCHAR(200) = NULL
            , @OldDateOfBirth NVARCHAR(200) = NULL

            SELECT @UsersId = [recordid]
            , @OldFirstName = [firstname]
            , @OldLastName = [lastname]
            , @OldIDNumber = [IDNumber]
            , @OldEmail = [email]
            , @OldMobile = [mobile]
            , @OldGender = [Gender]
            , @OldEthnicity = [Race]
            , @OldDateOfBirth =  CONVERT(NVARCHAR(50), [DateOfBirth], 120)
            , @isUpdate = CASE WHEN [firstname] != @FirstName OR
                                    [lastname] != @LastName OR
                                    ISNULL([IDNumber], '') != ISNULL(@IDNumber, '') OR
                                    [email] != @Email OR
                                    ISNULL([mobile], '') != ISNULL(@Mobile, '') OR
                                    ISNULL([Gender], '') != ISNULL(@Gender, '') OR
                                    ISNULL([Race], '') != ISNULL(@Ethnicity, '') OR
                                    ISNULL([DateOfBirth], '') != ISNULL(@DateOfBirth, '') THEN 1 ELSE 0 END
            FROM [Base].[dbo].[users]
            WHERE [UUID] = @UUID;

            IF @UsersId IS NULL
            BEGIN
                SET @UUID = NULL;
                SET @Message = 'User not found';
                RETURN 0;
            END

            -- Check email uniqueness for update (exclude current user)
            --IF EXISTS (
            --    SELECT 1 FROM [Base].[dbo].[users] u
            --    WHERE u.[email] = @Email 
            --    AND u.[recordid] != @UsersId
            --)
            --BEGIN
            --    SET @UUID = NULL;
            --    SET @Message = 'Email address already exists';
            --    RETURN 0;
            --END

            -- Check ID Number uniqueness if provided (exclude current user)
            IF @IDNumber IS NOT NULL AND @IDNumber != ''
            BEGIN
                IF EXISTS (
                    SELECT 1 FROM [Base].[dbo].[users] u
                    WHERE u.[IDNumber] = @IDNumber 
                        AND u.[recordid] != @UsersId
                        AND u.[isdeleted] = 0
                )
                BEGIN
                    SET @UUID = NULL;
                    SET @Message = 'ID Number already exists';
                    RETURN 0;
                END
            END

            IF @isUpdate = 1
            BEGIN
                -- Update the record
                UPDATE [Base].[dbo].[users]
                SET [firstname] = @FirstName,
                    [lastname] = @LastName,
                    [IDNumber] = @IDNumber,
                    [email] = @Email,
                    [mobile] = @Mobile,
                    [Gender] = @Gender,
                    [Race] = @Ethnicity,
                    [DateOfBirth] = @DateOfBirth
                WHERE [UUID] = @UUID;

                -- Log the changes
                EXEC [audit].[spAuditLogs_Log_Update] @CompanyId, 
                                                      @Usersid, 
                                                      'users', 
                                                      @UsersId, 
                                                      NULL, 
                                                      @AuditLogsid OUTPUT;

                -- Save audit details for changed fields
                IF @OldFirstName != @FirstName
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'firstname', @OldFirstName;

                IF @OldLastName != @LastName
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'lastname', @OldLastName;

                IF ISNULL(@OldIDNumber, '') != ISNULL(@IDNumber, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'IDNumber', @OldIDNumber;

                IF @OldEmail != @Email
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'email', @OldEmail;

                IF ISNULL(@OldMobile, '') != ISNULL(@Mobile, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'mobile', @OldMobile;

                IF ISNULL(@OldGender, '') != ISNULL(@Gender, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Gender', @OldGender;

                IF ISNULL(@OldEthnicity, '') != ISNULL(@Ethnicity, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'Race', @OldEthnicity;

                IF ISNULL(@OldDateOfBirth, '') != ISNULL(@DateOfBirth, '')
                    EXEC [audit].[spAuditLogDetails_Save] @AuditLogsid, 'DateOfBirth', @OldDateOfBirth;

            END
        
            SET @isSuccess = 1;
            SET @Message = 'User updated successfully';
        END
    END
    ELSE
    BEGIN
        SET @UUID = NULL;
        SET @isSuccess = 0;
        SET @Message = 'Access denied';
        RETURN 0;
    END
END