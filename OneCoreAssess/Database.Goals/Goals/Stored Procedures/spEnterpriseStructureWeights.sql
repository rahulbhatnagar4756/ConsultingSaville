CREATE PROCEDURE [Goals].[spEnterpriseStructureWeights]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT  esw.[UUID]
    , est.[UUID] [EnterpriseStructureTypesUUID]
    , est.[Name] [EnterpriseStructureTypes]
    , l.[UUID] [EmployeeLevelsUUID]
    , l.[Name] [EmployeeLevels]
    , esw.[Weight]
    FROM [Goals].[EnterpriseStructureWeights] esw
    INNER JOIN [Goals].[EnterpriseStructureTypes] est ON est.Id = esw.EnterpriseStructureTypesid
        AND esw.[isDeleted] = 0
    INNER JOIN [base].[Companies] c ON c.[recordID] = est.[Companyid]
        AND c.[UUID] = @CompanyUUID
    INNER JOIN [base].[EmployeeLevels] l ON l.[Id] = esw.[EmployeeLevelsid]
    WHERE est.isDeleted = 0
    ORDER BY est.Name, esw.EmployeeLevelsid;

END
