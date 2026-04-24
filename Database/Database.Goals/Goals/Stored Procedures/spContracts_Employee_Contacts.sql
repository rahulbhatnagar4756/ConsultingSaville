
CREATE PROCEDURE [Goals].[spContracts_Employee_Contacts]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @UsersUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
   SELECT c.[ContractsUUID]
        , p.[UUID] AS [ContractPeriodsUUID]
        , p.[Name] AS [ContractPeriods] 
        , p.[Description]
        , p.[DateStart]
        , p.[DateEnd]
        , p.[Year]
        , CONVERT(BIT, CASE WHEN c.[ContractsUUID] IS NULL THEN 0 ELSE 1 END) [HasContract]
        , p.[isActive] 
    FROM [Goals].[ContractPeriods] p
    INNER JOIN [base].[Companies] cp ON cp.[recordID] = p.[Companyid] AND cp.[UUID] = @CompanyUUID
    LEFT OUTER JOIN [Goals].[vwContracts_Employees] c ON c.[ContractPeriodsid] = p.[Id] AND c.[usersUUID] = @UsersUUID
    WHERE p.[isDeleted] = 0
    AND (p.[isActive] = 1 OR c.[Contractsid] IS NOT NULL)

END