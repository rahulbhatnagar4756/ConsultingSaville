



CREATE PROCEDURE [admin].[spIcons]
    @CompanyUUID NVARCHAR(200)
    , @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN

    SELECT [Name]
    FROM [admin].[Icons]
    WHERE [isDeleted] = 0
    ORDER BY [Name]

END
