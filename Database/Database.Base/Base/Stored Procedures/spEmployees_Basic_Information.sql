CREATE PROCEDURE [base].[spEmployees_Basic_Information]
	@CompanyUUID nvarchar(200) 
	, @UsersUUIDLoggedIn nvarchar(200) 
	, @UsersUUID nvarchar(200) 
AS
BEGIN

   SELECT uc.[Id] [UserCompanyUUID]
      , uc.[CompanyUUID] 
      , uc.[UsersUUID] 
      , ISNULL(uc.[EmployeeNo], UC.[EmployeeNumber]) [EmployeeNumber] 
      , uc.[DateStarted] [DateOfEmployment]
      , uc.[ContactNumber] [PhoneNumber]
      , uc.[isActive]
   FROM [base].[vwUserscompanys] uc 
   WHERE uc.[CompanyUUID] = @CompanyUUID AND uc.[UsersUUID] = @UsersUUID AND uc.[isDeleted] = 0

END
