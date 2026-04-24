CREATE PROCEDURE [secure].[spUsers_Login]
    @CompanyUUID NVARCHAR(250) = NULL,
    @Username NVARCHAR(50),
    @Password NVARCHAR(150),
    @isSuccessful BIT = 0 OUTPUT ,
    @Message NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @UsersId     INT = NULL
    , @CompanyId         INT = NULL
    , @CompanyIdSupplied INT = NULL
    , @LoginStatus       INT = 3 -- Default to Access Failed
    , @StatusSuccessful  INT = 1
    , @StatusFailed      INT = 3
    , @isLoginSuccessful BIT = 0
    , @isPasswordCorrect BIT = 0
    , @isCompanyAccess   BIT = 0
    , @ErrorMessage  NVARCHAR(500) = ''
    , @Json NVARCHAR(MAX) 

    BEGIN TRY
        -- Authenticate user (case-sensitive password check)
        SELECT TOP(1) @UsersId = u.[recordid]
        , @CompanyId = uc.[Companyid]
        , @isPasswordCorrect = CASE WHEN u.[password] = @Password COLLATE SQL_Latin1_General_CP1_CS_AS THEN 1 ELSE 0 END
        , @isCompanyAccess = CASE WHEN uc.[Id] IS NULL AND @CompanyUUID IS NOT NULL THEN 0 ELSE 1 END
        FROM [dbo].[users] u
        LEFT OUTER JOIN [dbo].[vwUsersCompanys] uc ON uc.[Usersid] = u.[recordid]
            AND uc.[isDeleted] = 0
            AND uc.[isActive] = 1
            AND CASE WHEN @CompanyUUID IS NULL THEN '' ELSE uc.[CompanyUUID] END = ISNULL(@CompanyUUID, '')
        WHERE u.[username] = @Username 
          AND u.[isdeleted] = 0
          AND u.[isactive] = 1
        ORDER BY CASE WHEN uc.[Id] IS NULL THEN 100 ELSE 1 END;

        
        --set if login was successful or failed
        IF @UsersId IS NOT NULL
        BEGIN

            --get the company
            IF @CompanyUUID IS NOT NULL
                SELECT @CompanyIdSupplied = [Recordid]
                FROM [dbo].[Companies] c
                WHERE c.[UUID] = @CompanyUUID; 

            SET @LoginStatus = CASE WHEN @isPasswordCorrect = 0 OR @isCompanyAccess = 0 THEN @StatusFailed 
                        ELSE @StatusSuccessful END

            INSERT [secure].[LogUserLogins] ([Usersid], [Companyid], [Statusid])
            VALUES (@UsersId, ISNULL(@CompanyId, @CompanyIdSupplied), @LoginStatus)

            IF @isPasswordCorrect = 0 OR @isCompanyAccess = 0
            BEGIN
                IF @isPasswordCorrect = 0 
                    SET @Message = 'Invalid username or password'
                ELSE
                    SET @Message = 'User does not have access to the specified company'

                SELECT NULL [Json];
                RETURN 0;
            END

            SET @Json = (
            SELECT u.[UUID] AS [UsersUUID]
            , ul.[CompanyUUID] [CompanyUUIDLastLoggedIn]
            , (
                SELECT uc.[CompanyUUID] 
                , uc.[Company]
                , (
                    SELECT r.[Name] [Roles]
                    FROM [dbo].[UsersRolesLinks] l
                    INNER JOIN [dbo].[UsersRoles] r ON r.[Id] = l.[UsersRolesid]
                        AND r.[isDeleted] = 0
                    WHERE l.[Usersid] = uc.[Usersid] 
                        AND l.[isDeleted] = 0
                        AND l.[Companyid] = uc.[Companyid]
                     FOR JSON AUTO
                  ) AS [Roles]
                FROM [dbo].[vwUsersCompanys] uc
                WHERE uc.[Usersid] = u.[recordid] 
                FOR JSON AUTO
              ) AS [Company]
           

            FROM [dbo].[users] u
            LEFT OUTER JOIN [secure].[vwLogUserLogins] ul ON ul.[Usersid] = u.[recordid] 
                AND ul.[No] = 1
            WHERE u.[recordid] = @UsersId
            FOR JSON PATH, ROOT('Data'));

            SET @isSuccessful = 1;
            SET @Message = 'Login Successful';
            SELECT @Json [JsonResult];
            RETURN 1;

        END 
        ELSE 
        BEGIN
            SET @Message = 'Invalid username or password';

            SELECT NULL [JsonResult];
            RETURN 0;
        END
         

    END TRY
    BEGIN CATCH
        SET @Message = 'Login error: ' + ERROR_MESSAGE();
        
        SELECT NULL [JsonResult];
        RETURN 0;
    END CATCH
END