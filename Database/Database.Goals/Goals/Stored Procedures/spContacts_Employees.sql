USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spContacts_Employees]    Script Date: 16/12/2025 17:48:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE ALTER PROCEDURE [Goals].[spContacts_Employees] 
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @json NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF LEN(@json) = 0
        SET @json = '{}';

    ------------------------------------------------------------
    -- VARIABLES
    ------------------------------------------------------------
    DECLARE @Search NVARCHAR(350) = '',
            @isIncludeTeam BIT = 0,
            @isDepartment BIT = 0,
            @isBusinessUnitTypes BIT = 0,
            @isBusinessUnits BIT = 0,
            @isPosition BIT = 0,
            @isLevel BIT = 0,
            @isCriticalRoles BIT = 0,
            @isDisciplines BIT = 0,
            @isGender BIT = 0,
            @isEthnicity BIT = 0,
            @isYearsOfExperience BIT = 0,
            @isAge BIT = 0;

    ------------------------------------------------------------
    -- 🔹 ROLE VARIABLES (ADDED)
    ------------------------------------------------------------
    DECLARE @Usersid INT,
            @CompaniesId INT,
            @IsAdminEmployee BIT = 0;

    ------------------------------------------------------------
    -- SEARCH & INCLUDE TEAM
    ------------------------------------------------------------
    SELECT  
        @Search = ISNULL(MAX(CASE WHEN [Keys] = 'Search' THEN [Data] END), ''),
        @isIncludeTeam = ISNULL(
            CASE 
                WHEN ISNULL(MAX(CASE WHEN [Keys] = 'isIncludeTeam' THEN [Data] END), 'false') = 'true' 
                THEN 1 ELSE 0 
            END, 0)
    FROM (
        SELECT [Key] AS [Keys], [Value] AS [Data]
        FROM OPENJSON(@json)
        WHERE [Key] IN ('Search','isIncludeTeam')
    ) x;

    ------------------------------------------------------------
    -- FILTER FLAGS
    ------------------------------------------------------------
    SELECT @isBusinessUnitTypes = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END FROM OPENJSON(@json, '$.BusinessUnitTypes');
    SELECT @isBusinessUnits     = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END FROM OPENJSON(@json, '$.BusinessUnits');
    SELECT @isDepartment        = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END FROM OPENJSON(@json, '$.Departments');
    SELECT @isPosition          = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END FROM OPENJSON(@json, '$.Position');
    SELECT @isLevel             = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END FROM OPENJSON(@json, '$.Level');
    SELECT @isCriticalRoles     = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END FROM OPENJSON(@json, '$.CriticalRoles');
    SELECT @isDisciplines        = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END FROM OPENJSON(@json, '$.Disciplines');
    SELECT @isGender             = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END FROM OPENJSON(@json, '$.Gender');
    SELECT @isEthnicity          = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END FROM OPENJSON(@json, '$.Ethnicity');
    SELECT @isYearsOfExperience  = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END FROM OPENJSON(@json, '$.YearsOfExperience');
    SELECT @isAge                = CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END FROM OPENJSON(@json, '$.Age');

    ------------------------------------------------------------
    -- 🔹 USER & COMPANY IDS (ADDED)
    ------------------------------------------------------------
    SELECT @Usersid = recordid
    FROM [BASE].[dbo].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn;


    SELECT @CompaniesId = recordID
    FROM [BASE].[dbo].[Companies]
    WHERE [UUID] = @CompanyUUID;

    ------------------------------------------------------------
    -- 🔹 CHECK AdminEmployee ROLE (ADDED)
    ------------------------------------------------------------
    IF EXISTS (
        SELECT 1
        FROM [BASE].[dbo].[UsersRoles] r
        INNER JOIN [BASE].[dbo].[UsersRolesLinks] rl 
            ON rl.UsersRolesid = r.Id
           AND rl.isDeleted = 0
        WHERE r.Name = 'AdminEmployee'
          AND rl.Usersid = @Usersid
          AND rl.Companyid = @CompaniesId
    )
    BEGIN
        SET @IsAdminEmployee = 1;
    END

    ------------------------------------------------------------
    -- MAIN QUERY (UNCHANGED)
    ------------------------------------------------------------
    SELECT 
        e.[CompanyUUID],
        e.[UsersUUID],
        e.[UsersUUIDManager],
        e.[EmployeeJobsUUID],
        e.[EmployeeDepartmentsUUID],
        e.[EmployeeBusinessUnitsUUID],
        e.[EmployeeBusinessUnitTypesUUID],
        e.[EmployeeLevelsUUID],
        e.[EmployeeBusinessUnitTypes],
        e.[EmployeeJobsCriticalRolesUUID],
        e.[EmployeeJobDisciplinesUUID],
        e.[EmployeeBusinessUnits],
        e.[EmployeeDepartments],
        e.[EmployeeJobs],
        e.[EmployeeLevels],
        e.[LevelOrder],
        e.[EmployeeJobsCriticalRoles],
        e.[EmployeeJobDisciplines],
        e.[FirstName],
        e.[MiddleName],
        e.[LastName],
        e.[Email],
        e.[Mobile],
        e.[IDNumber],
        e.[EmployeeNumber],
        e.[Race],
        e.[Gender],
        e.[IsImage],
        e.[Image],
        e.[Age],
        e.[DateOfBirth],
        e.[TandCsigned],
        e.[FullnameManager],
        e.[EmailManager],
        e.[IDNumberManager],
        e.[EmployeeNumberManager],
        e.[IsDeletedUsersManager],
        CONVERT(BIT, CASE WHEN c.[Id] IS NULL THEN 0 ELSE 1 END) AS isValidContract
    FROM [base].[vwEmployees] e
    INNER JOIN [base].[vwEmployeesCorporateAccessLevels] ecal 
        ON ecal.UserUUID = @UsersUUIDLoggedIn
       AND ecal.Companyid = e.Companyid
       AND ecal.isActive = 1
       AND ecal.isDeleted = 0
       AND (
            ecal.isFullAccess = 1
            OR (
                ISNULL(ecal.EmployeeBusinessUnitTypesid, 0) =
                CASE WHEN ecal.EmployeeBusinessUnitTypesid IS NULL THEN 0 ELSE e.EmployeeBusinessUnitTypesid END
                AND ISNULL(ecal.EmployeeBusinessUnitsid, 0) =
                CASE WHEN ecal.EmployeeBusinessUnitsid IS NULL THEN 0 ELSE e.EmployeeBusinessUnitsid END
                AND ISNULL(ecal.EmployeeDepartmentsid, 0) =
                CASE WHEN ecal.EmployeeDepartmentsid IS NULL THEN 0 ELSE e.EmployeeDepartmentsid END
            )
        )
    LEFT JOIN [Goals].[vwContracts_Users] c 
        ON c.Usersid = e.Usersid
       AND c.isActive = 1
       AND c.isDeleted = 0
       AND c.No = 1
    WHERE e.No = 1
      AND e.CompanyUUID = @CompanyUUID

      ------------------------------------------------------------
      -- 🔹 ROLE FILTER (SAFE & ADDITIVE)
      ------------------------------------------------------------
      AND (
            @IsAdminEmployee = 0
            OR (
                @IsAdminEmployee = 1
                AND e.UsersidManager = @Usersid
                AND e.Companyid = @CompaniesId
            )
          )

      ------------------------------------------------------------
      -- 🔹 EXISTING FILTERS (UNCHANGED)
      ------------------------------------------------------------
      AND (@isBusinessUnitTypes = 0 OR e.EmployeeBusinessUnitTypesUUID IN (SELECT value FROM OPENJSON(@json, '$.BusinessUnitTypes')))
      AND (@isBusinessUnits     = 0 OR e.EmployeeBusinessUnitsUUID IN (SELECT value FROM OPENJSON(@json, '$.BusinessUnits')))
      AND (@isDepartment        = 0 OR e.EmployeeDepartmentsUUID IN (SELECT value FROM OPENJSON(@json, '$.Departments')))
      AND (@isPosition          = 0 OR e.EmployeeJobsUUID IN (SELECT value FROM OPENJSON(@json, '$.Position')))
      AND (@isLevel             = 0 OR e.EmployeeLevelsUUID IN (SELECT value FROM OPENJSON(@json, '$.Level')))
      AND (@isCriticalRoles     = 0 OR e.EmployeeJobsCriticalRolesUUID IN (SELECT value FROM OPENJSON(@json, '$.CriticalRoles')))
      AND (@isDisciplines        = 0 OR e.EmployeeJobDisciplinesUUID IN (SELECT value FROM OPENJSON(@json, '$.Disciplines')))
      AND (@isGender             = 0 OR e.Gender IN (SELECT value FROM OPENJSON(@json, '$.Gender')))
      AND (@isEthnicity          = 0 OR e.Race IN (SELECT value FROM OPENJSON(@json, '$.Ethnicity')))
      AND (@isAge                = 0 OR e.Age IN (SELECT value FROM OPENJSON(@json, '$.Age')))

      AND (
            ISNULL(e.firstname,'') + ' ' + ISNULL(e.lastname,'') + ' ' +
            ISNULL(e.IDNumber,'') + ' ' + ISNULL(e.EmployeeNumber,'') + ' ' +
            ISNULL(e.email,'') + ' ' + ISNULL(e.mobile,'') + ' ' +
            ISNULL(CASE WHEN @isIncludeTeam = 1 THEN e.FullnameManager END,'')
          ) LIKE '%' + @Search + '%'

    ------------------------------------------------------------
    -- 🔹 GROUP BY (UNCHANGED)
    ------------------------------------------------------------
    GROUP BY 
        e.[CompanyUUID], e.[UsersUUID], e.[UsersUUIDManager],
        e.[EmployeeJobsUUID], e.[EmployeeDepartmentsUUID],
        e.[EmployeeBusinessUnitsUUID], e.[EmployeeBusinessUnitTypesUUID],
        e.[EmployeeLevelsUUID], e.[EmployeeBusinessUnitTypes],
        e.[EmployeeJobsCriticalRolesUUID], e.[EmployeeJobDisciplinesUUID],
        e.[EmployeeBusinessUnits], e.[EmployeeDepartments],
        e.[EmployeeJobs], e.[EmployeeLevels],
        e.[EmployeeJobsCriticalRoles], e.[EmployeeJobDisciplines],
        e.[FirstName], e.[MiddleName], e.[LastName],
        e.[Email], e.[Mobile], e.[IDNumber], e.[EmployeeNumber],
        e.[Race], e.[Gender], e.[IsImage], e.[Image],
        e.[Age], e.[DateOfBirth], e.[TandCsigned],
        e.[FullnameManager], e.[EmailManager],
        e.[IDNumberManager], e.[EmployeeNumberManager],
        e.[IsDeletedUsersManager],
        c.[Id],
        e.[Usersid], e.[LevelOrder];
END
