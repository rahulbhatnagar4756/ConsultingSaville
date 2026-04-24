 --	@CompanyUUID NVARCHAR(200) = 'C1906F1D-38BE-40FF-A7D0-5C24B8AC5732'
	--, @UsersUUIDLoggedIn NVARCHAR(200) = ''
	--, @UsersUUID NVARCHAR(200) = 'E2A0D196-C852-4F23-890D-56D289205D8E'
	--, @ContractPeriodsUUID NVARCHAR(200) = 'AD57902A-628F-498C-B99A-8AD2CDF487F4'
CREATE PROCEDURE [Goals].[spContracts_User_ContractPeriod]
	@CompanyUUID NVARCHAR(200) 
	, @UsersUUIDLoggedIn NVARCHAR(200) 
	, @UsersUUID NVARCHAR(200) 
	, @ContractPeriodsUUID NVARCHAR(200) 
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ContractsUUID NVARCHAR(200) = NULL

	SELECT TOP(1) @ContractsUUID = c.[ContractsUUID]
	FROM [Goals].[vwContracts_Employees] c
	WHERE c.[usersUUID] = @UsersUUID
		AND c.[ContractPeriodsUUID] = @ContractPeriodsUUID
		 

	EXEC [Goals].[spContracts]	@CompanyUUID, @UsersUUIDLoggedIn, @ContractsUUID  
					 
END