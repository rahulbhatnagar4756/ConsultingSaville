CREATE PROCEDURE [Goals].[spTemplates_Information]
    @CompanyUUID NVARCHAR(200) --= 'C1906F1D-38BE-40FF-A7D0-5C24B8AC5732'
    , @UsersUUIDLoggedIn NVARCHAR(200)
AS
BEGIN

    SET NOCOUNT ON;
    DECLARE  @json NVARCHAR(MAX);

   
    

        SET @json = (
        SELECT   
            t.[UUID] AS [TemplatesUUID],
            t.[Name],
            t.[Code],
            (SELECT c.[UUID] AS [ContractsUUID]
             , p.[UUID] AS [ContractPeriodsUUID]
             , p.[Name] AS [ContractPeriods] 
             , p.[Description]
             , p.[DateStart]
             , p.[DateEnd]
             , p.[Year]
             , c.[isDepartmentTemplateSync]
             , c.[isIndividualTemplateSync]
             , p.[DateTerminationActive]
             , p.[isActive] 
             

             FROM [Goals].[ContractPeriods] p
             INNER JOIN [base].[Companies] cp ON cp.[recordID] = p.[Companyid] AND cp.[UUID] = @CompanyUUID
             LEFT OUTER JOIN [Goals].[Contracts] c ON c.[ContractPeriodsid] = p.[Id] AND c.[Templatesid] = t.[Id]
             WHERE p.[isDeleted] = 0
             AND (p.[isActive] = 1 OR c.[Id] IS NOT NULL)
             FOR JSON PATH) AS [Contracts],

            (
                SELECT  
                tt.[UUID] [TemplateBusinessUnitDepartmentPositionsUUID],
                tt.[EmployeeBusinessUnitTypesUUID],
                tt.[EmployeeBusinessUnitTypes],
                tt.[EmployeeBusinessUnitsUUID],
                tt.[EmployeeBusinessUnits],
                tt.[EmployeeDepartmentsUUID],
                tt.[EmployeeDepartments],
                tt.[EmployeeJobsUUID],
                tt.[EmployeeJobs],
                tt.[EmployeeLevelsUUID],
                tt.[EmployeeLevels]
                FROM [Goals].[vwTemplateBusinessUnitDepartmentPositions] tt 
                WHERE tt.[Templatesid] = t.[Id]
                FOR JSON AUTO

             ) AS [BusinessUnitDepartmentPositions],

            (
                SELECT  
                u.[UsersUUID], 
                u.[firstname],
                u.[lastname],
                u.[email], 
                u.[mobile],
                u.[IDNumber],
                u.[Race],
                u.[Gender],
                u.[DateOfBirth],
                u.[UsersImageURL]
                FROM [Goals].[vwContracts_Employees] u
                WHERE u.[CompanyUUID] = @CompanyUUID 
                    AND u.[Templatesid] = t.[Id]
                    AND u.[isActiveContracts] = 1
                    AND u.[isActiveContractsSync] = 1
                GROUP BY u.[UsersUUID], u.[firstname], u.[lastname], u.[email], u.[mobile], 
                    u.[IDNumber], u.[Race], u.[Gender], u.[DateOfBirth], u.[UsersImageURL]
                FOR JSON AUTO

             ) AS [Employees]

        FROM [Goals].[Goals].[vwTemplates] t
        WHERE t.[CompanyUUID] = @CompanyUUID 
            AND t.[isDeleted] = 0
        FOR JSON PATH, ROOT('Data')
    );

    -- Return the JSON as a single row
    SELECT @json AS JsonResult;


END
