USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spEmployees_GetDetails]    Script Date: 27/10/2025 10:04:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [Goals].[spEmployees_GetDetails]
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
