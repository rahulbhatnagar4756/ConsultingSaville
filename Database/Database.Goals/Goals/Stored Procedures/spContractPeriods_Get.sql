CREATE PROCEDURE [Goals].[spContractPeriods_Get]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @ContractPeriodsUUID NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        cp.[UUID],
        cp.[Companyid],
        cp.[Name],
        cp.[Description],
        cp.[DateStart],
        cp.[DateEnd],
        cp.[Year],
        cp.[DateTerminationActive],
        cp.[isActive]
    FROM [Goals].[ContractPeriods] cp
    INNER JOIN [Base].[dbo].[Companies] c 
        ON cp.[Companyid] = c.[recordID] 
       AND c.[UUID] = @CompanyUUID
    WHERE cp.[isDeleted] = 0
      AND (@ContractPeriodsUUID IS NULL OR cp.[UUID] = @ContractPeriodsUUID);
END