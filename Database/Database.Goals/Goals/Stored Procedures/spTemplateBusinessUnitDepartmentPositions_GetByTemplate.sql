
-- TemplateBusinessUnitDepartmentPositions - Get by Template UUID
CREATE   PROCEDURE [Goals].[spTemplateBusinessUnitDepartmentPositions_GetByTemplate]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @TemplatesUUID NVARCHAR(200)

AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CompanyId INT
    , @UsersId INT
    , @isHasAccess BIT = 0
    , @SecurityRoleAccessidRead INT = 9

    -- Get Company ID
    SELECT @CompanyId = [recordID] 
    FROM [Base].[dbo].[Companies] 
    WHERE [UUID] = @CompanyUUID;
    
    IF ISNULL(@CompanyId, 0) = 0
    BEGIN
        SELECT NULL FOR JSON PATH
        RETURN 0;
    END

    -- Get User ID
    SELECT TOP(1) @UsersId = [Id]
    FROM [Base].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    IF ISNULL(@UsersId, 0) = 0
    BEGIN
        SELECT NULL FOR JSON PATH
        RETURN 0;
    END

    -- Check Access Rights
    EXEC @isHasAccess = [Base].[secure].[spSecurityUsersRoles_Has_Access] @UsersUUIDLoggedIn, @CompanyUUID, @SecurityRoleAccessidRead;

    SELECT tbdp.[UUID],
        tbdp.[EmployeeBusinessUnitTypesUUID],
        tbdp.[EmployeeBusinessUnitTypes],
        tbdp.[EmployeeBusinessUnitsUUID],
        tbdp.[EmployeeBusinessUnits],
        tbdp.[EmployeeDepartmentsUUID],
        tbdp.[EmployeeDepartments],
        tbdp.[EmployeeJobsUUID],
        tbdp.[EmployeeJobs],
        tbdp.[EmployeeLevelsUUID],
        tbdp.[EmployeeLevels]
    FROM [Goals].[vwTemplateBusinessUnitDepartmentPositions] tbdp
    INNER JOIN [Goals].[Templates] t ON t.[Id] = tbdp.[Templatesid] 
        AND t.[Companyid] = @CompanyId
        AND t.[isDeleted] = 0
        AND t.[UUID] = @TemplatesUUID
    WHERE @isHasAccess = 1
        
    ORDER BY 
        tbdp.[EmployeeBusinessUnits],
        tbdp.[EmployeeDepartments],
        tbdp.[EmployeeJobs]
       
       
    
END

