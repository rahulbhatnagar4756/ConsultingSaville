CREATE   PROCEDURE [Goals].[sp_IsUserManager]
    @UsersUUID UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsManager BIT = 0;

    -- Check if this user UUID appears as a manager anywhere
    IF EXISTS (
        SELECT 1
        FROM [Goals].[Base].[vwEmployees]
        WHERE UsersUUIDManager = @UsersUUID
    )
        SET @IsManager = 1;

    -- Return result
    SELECT @IsManager AS IsManager;
END