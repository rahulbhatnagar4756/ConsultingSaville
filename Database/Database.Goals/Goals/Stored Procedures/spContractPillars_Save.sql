
CREATE PROCEDURE [Goals].[spContractPillars_Save]
    @ContractsUUID NVARCHAR(200) 
AS
BEGIN

    SET NOCOUNT ON;
    DECLARE @Contractsid INT = 0
    , @Companyid INT = 0;

    SELECT @Contractsid = [Id], @Companyid = c.[Companyid]
    FROM [Goals].[Contracts] c
    WHERE c.[UUID] = @ContractsUUID

    INSERT INTO [Goals].[ContractPillars]
       (
           [Contractid]
           , [EnterpriseStructureTypesid]
           , [Pillarsid]
           , [Weight]
       )

    SELECT @Contractsid
        , x.[EnterpriseStructureTypesid]
        , x.[Pillarsid]
        , 100 [Weight]
    FROM (
           SELECT es.[id] [EnterpriseStructureTypesid]
            , p.[Id] [Pillarsid]
            FROM [Goals].[EnterpriseStructureTypes] es
            CROSS JOIN [Goals].[Pillars] p 
            WHERE es.[Companyid] = @Companyid
                AND es.[isDeleted] = 0 
                AND P.[Companyid] = @Companyid 
                AND P.[isDeleted] = 0
        ) x
    LEFT OUTER JOIN [Goals].[ContractPillars] cp ON cp.[EnterpriseStructureTypesid] = x.[EnterpriseStructureTypesid]
        AND cp.[Pillarsid] = x.[Pillarsid]
        AND cp.[isDeleted] = 0
        AND cp.[Contractid] = @Contractsid 
    WHERE cp.[Id] IS NULL         

END