CREATE PROCEDURE [Base].[spUsers_Basic]
    @CompanyUUID        NVARCHAR(200),
    @UsersUUIDLoggedin  NVARCHAR(200),
    @UsersUUID          NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE  @UsersId        INT
           , @DateOfBirth    DATETIME = NULL
           , @URLAddress     NVARCHAR(360)
           , @RootPath       NVARCHAR(400)
           , @AvatarPath     NVARCHAR(500)
           , @DefaultPath    NVARCHAR(500)
           , @ResolvedPath   NVARCHAR(500)
           , @IDNumber       NVARCHAR(300)
           , @FileExists     INT
           , @Sql            NVARCHAR(MAX);

    -- Get base data & build expected paths
    SELECT TOP (1)
          @UsersId     = u.[recordid]
        , @DateOfBirth = u.[DateOfBirth]
        , @IDNumber    = u.[IDNumber]
        , @RootPath    = c.[RootPath]
        , @URLAddress  = c.[URLAddress]
        , @AvatarPath  = c.[RootPath] + '\Images\Avatars\' + CONVERT(NVARCHAR(20), u.[recordid]) + 'medium.jpg'
        , @DefaultPath = c.[RootPath] + '\Images\Avatars\anonymous-avatar.jpg'
    FROM [dbo].[users] u
    CROSS JOIN [dbo].[Companies] c
    WHERE c.[UUID] = @CompanyUUID
      AND u.[UUID] = @UsersUUID;

    IF @UsersId IS NULL
    BEGIN
        -- No matching user
        SELECT CAST(NULL AS NVARCHAR(200)) AS [UUID],
               CAST(NULL AS NVARCHAR(150)) AS [FirstName],
               CAST(NULL AS NVARCHAR(150)) AS [LastName],
               CAST(NULL AS NVARCHAR(50))  AS [IDNumber],
               CAST(NULL AS NVARCHAR(250)) AS [Email],
               CAST(NULL AS NVARCHAR(50))  AS [Mobile],
               CAST(NULL AS NVARCHAR(50))  AS [Gender],
               CAST(NULL AS NVARCHAR(50))  AS [Ethnicity],
               CAST(NULL AS DATETIME)      AS [DateOfBirth],
               CAST(NULL AS NVARCHAR(500)) AS [UsersImageURL],
               CAST(NULL AS VARBINARY(MAX)) AS [UsersImageBytes];
        RETURN;
    END

    -- Check if avatar file exists
    EXEC xp_fileexist @AvatarPath, @FileExists OUTPUT;

    -- Resolve which path to load
    SET @ResolvedPath = CASE WHEN @FileExists = 1 THEN @AvatarPath ELSE @DefaultPath END;

    -- Attempt to infer & persist DateOfBirth from SA ID if missing
    IF @DateOfBirth IS NULL AND @IDNumber IS NOT NULL
    BEGIN
        SET @DateOfBirth = [base].[func_Tool_SAIDNumber_ReturnBirthDate] (@IDNumber);

        IF @DateOfBirth IS NOT NULL
        BEGIN
            UPDATE [dbo].[users]
            SET [DateOfBirth] = @DateOfBirth
            WHERE [recordid] = @UsersId;
        END
    END

    DECLARE @ImageBytes VARBINARY(MAX) = NULL;

    -- Load image bytes (requires Ad Hoc Distributed Queries + file system access)
    BEGIN TRY
        DECLARE @Image TABLE (Img VARBINARY(MAX));
        SET @Sql = N'SELECT BulkColumn FROM OPENROWSET(BULK N''' +
                   REPLACE(@ResolvedPath, '''', '''''') +
                   ''', SINGLE_BLOB) AS IMG;';
        INSERT INTO @Image EXEC (@Sql);
        SELECT TOP (1) @ImageBytes = Img FROM @Image;
    END TRY
    BEGIN CATCH
        -- Swallow errors; leave @ImageBytes = NULL if cannot read
    END CATCH;

    SELECT u.[UUID]
         , u.[firstname]              AS [FirstName]
         , u.[lastname]               AS [LastName]
         , u.[IDNumber]
         , u.[email]                  AS [Email]
         , u.[mobile]                 AS [Mobile]
         , u.[Gender]
         , u.[Race]                   AS [Ethnicity]
         , u.[DateOfBirth]
         , CASE WHEN @FileExists = 1
                THEN @URLAddress + 'Images/Avatars/' + CONVERT(NVARCHAR(20), @UsersId) + 'medium.jpg'
                ELSE @URLAddress + 'Images/Avatars/anonymous-avatar.jpg'
           END                        AS [UsersImageURL]
         , @ImageBytes                AS [UsersImageBytes]
    FROM [dbo].[users] u
    WHERE u.[recordid] = @UsersId;
END