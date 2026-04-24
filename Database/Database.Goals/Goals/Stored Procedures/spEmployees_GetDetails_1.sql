CREATE   PROCEDURE [Goals].[spEmployees_GetDetails]
(
    @UsersUUID UNIQUEIDENTIFIER
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        [UsersUUID],
        [CompanyUUID],
        [firstname] AS FirstName,
        [MiddleName],
        [lastname] AS LastName,
        [email],
        [mobile],
        [EmployeeNumber],
        [EmployeeDepartments],
        [EmployeeJobs],
        [EmployeeLevels],
        [FullnameManager],
        [emailManager],
        [IDNumber],
        [IDNumberManager],
        [EmployeeBusinessUnits],
        [EmployeeDepartments],
        [EmployeeJobs],
        [DateOfBirth],
        [Age],
        [isImage] AS IsImage,
        [Image]
    FROM [Goals].[Base].[vwEmployees]
    WHERE UsersUUID = @UsersUUID
      AND isDeletedUsersManager = 0;
END