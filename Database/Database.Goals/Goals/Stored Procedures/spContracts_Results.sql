CREATE OR ALTER PROCEDURE [Goals].[spContracts_Results]
    @CompanyUUID NVARCHAR(200),
    @UsersUUIDLoggedIn NVARCHAR(200),
    @ContractsUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @json NVARCHAR(MAX),
        @Companyid INT = 0,
        @Contractsid INT = 0,
        @Usersid INT,
        @IsManager BIT = 0,
        @IsAdmin BIT = 0;

    -----------------------------
    -- Resolve Company and Contract IDs
    -----------------------------
    SELECT @Companyid = [Recordid]
    FROM [Base].[Companies]
    WHERE [UUID] = @CompanyUUID; 

    SELECT @Contractsid = [id]
    FROM [Goals].[Contracts]
    WHERE [UUID] = @ContractsUUID;

    -----------------------------
    -- Resolve UsersID from UUID
    -----------------------------
    SELECT @Usersid = [recordid]
    FROM [BASE].[dbo].[Users]
    WHERE [UUID] = @UsersUUIDLoggedIn;

    -----------------------------
    -- Determine Roles
    -----------------------------
    SELECT @IsManager = CASE WHEN EXISTS (
        SELECT 1
        FROM [BASE].[dbo].[UsersRoles] r
        INNER JOIN [BASE].[dbo].[UsersRolesLinks] rl 
            ON rl.[UsersRolesid] = r.[Id] AND rl.[isDeleted] = 0
        INNER JOIN [BASE].[dbo].[Users] u 
            ON u.[recordid] = rl.[Usersid] AND u.[recordid] = @Usersid
        INNER JOIN [BASE].[dbo].[Companies] c 
            ON c.[recordID] = rl.[Companyid] AND c.[UUID] = @CompanyUUID
        WHERE r.[Name] = 'SystemAdministrator' AND r.[isDeleted] = 0
    ) THEN 1 ELSE 0 END;

    SELECT @IsAdmin = CASE WHEN EXISTS (
        SELECT 1
        FROM [BASE].[dbo].[UsersRoles] r
        INNER JOIN [BASE].[dbo].[UsersRolesLinks] rl 
            ON rl.[UsersRolesid] = r.[Id] AND rl.[isDeleted] = 0
        INNER JOIN [BASE].[dbo].[Users] u 
            ON u.[recordid] = rl.[Usersid] AND u.[recordid] = @Usersid
        INNER JOIN [BASE].[dbo].[Companies] c 
            ON c.[recordID] = rl.[Companyid] AND c.[UUID] = @CompanyUUID
        WHERE r.[Name] = 'Admin' AND r.[isDeleted] = 0
    ) THEN 1 ELSE 0 END;

    -----------------------------
    -- Ensure ContractPillars exist
    -----------------------------
    EXEC [Goals].[spContractPillars_Save] @ContractsUUID;

    -----------------------------
    -- Generate JSON
    -----------------------------
    SET @json = (
        SELECT 
            c.[UUID] AS [ContractsUUID],
            com.[UUID] AS [CompanyUUID],
            com.[Name] AS [Company],
            u.[UUID] AS [UsersUUID],
            t.[UUID] AS [TemplatesUUID],
            CASE 
                WHEN c.[Usersid] IS NOT NULL THEN u.[Firstname] + ISNULL(' ' + u.[Lastname],'')
                ELSE CASE WHEN c.Templatesid IS NOT NULL THEN t.[Name] + ISNULL(' - ' + t.[Code],'') ELSE '' END
            END AS [ContractOwnerName],
            cp.[UUID] AS [ContractPeriodsUUID],
            cp.[Name] AS [ContractPeriods],
            cp.[DateStart],
            cp.[DateEnd],
            CASE WHEN c.[Usersid] IS NULL THEN 0 ELSE 1 END AS [isUser],
            CASE WHEN c.Templatesid IS NULL THEN 0 ELSE 1 END AS [isTemplate],
            c.[isActive],
            c.[isDeleted],

            -- Enterprise Structures & Pillars
            (
                SELECT est.[UUID],
                       est.[Name],
                       est.[Icons],
                       est.[IconColor],
                       (
                           SELECT p.[UUID],
                                  cp.[UUID] AS [ContractPillarsUUID],
                                  p.[Name],
                                  p.[Icons],
                                  p.[IconColor],
                                  cp.[Weight],
                                  -- KPAs
                                  (
                                      SELECT kpa.[UUID],
                                             cp.[UUID] AS [ContractPillarsUUID],
                                             kpa.[StatusUUID],
                                             kpa.[Status],
                                             kpa.[RatingPeriodsUUID],
                                             kpa.[RatingPeriods],
                                             kpa.[RatingPeriodsDisplay],
                                             'Q3' AS [RatingPeriodDates],
                                             kpa.[Name],
                                             kpa.[Description],
                                             cpk.[Weight],
                                             kpa.[isActive],
                                             'Template 1' AS [Template],
                                             'Level 3' AS [StatusScore],
                                             '#c30010' AS [StatusScoreColor],
                                             50.00 AS [ScoreEmployee],
                                             50.00 AS [ScoreManager],

                                             -- KPI for user
                                             (
                                                 SELECT kpi.[UUID],
                                                        kpa.[UUID] AS [KPAUUID],
                                                        l.[UUID] AS [KPAKPIUUID],
                                                        kpi.[CompanyUUID],
                                                        kpi.[UsersUUID],
                                                        kpa.[RatingPeriodsUUID],
                                                        kpa.[RatingPeriods],
                                                        kpa.[RatingPeriodsDisplay],
                                                        'Q3' AS [RatingPeriodDates],
                                                        kpi.[StatusUUID],
                                                        kpi.[Status],
                                                        kpi.[ToleranceSetsUUID],
                                                        kpi.[ToleranceSets],
                                                        kpi.[ToleranceSetsDescription],
                                                        kpi.[Name],
                                                        kpi.[Description],
                                                        kpi.[DateStart],
                                                        kpi.[DateEnd],
                                                        kpi.[Target],
                                                        ISNULL(r.[Result],0) AS [CurrentResultProgress],
                                                        l.[Weight],
                                                        kpi.[isActive],
                                                        'Template 1' AS [Template],
                                                        'Level 3' AS [StatusScore],
                                                        '#c30010' AS [StatusScoreColor],
                                                        50.00 AS [ScoreEmployee],
                                                        50.00 AS [ScoreManager],
                                                        (
                                                            SELECT res.[DateAdded],
                                                                   res.[Result],
                                                                   res.[DateCreated],
                                                                   res.[isActive]
                                                            FROM [Goals].[Results] res
                                                            WHERE res.[KPIid] = kpi.[Id] AND res.[isDeleted] = 0
                                                            ORDER BY res.[DateAdded] ASC
                                                            FOR JSON PATH
                                                        ) AS [ProgressGraphData]
                                                 FROM [Goals].[vwKPI] kpi
                                                 INNER JOIN [Goals].[KPAKPI] l ON l.[KPIid] = kpi.[Id] AND l.[KPAid] = kpa.[Id] AND l.[isDeleted] = 0
                                                 LEFT JOIN (
                                                     SELECT [KPIid], [Result], [DateAdded]
                                                     FROM [Goals].[Results]
                                                     WHERE [isDeleted] = 0 AND [isActive] = 1
                                                     AND [Id] IN (
                                                         SELECT MAX([Id]) 
                                                         FROM [Goals].[Results]
                                                         WHERE [isDeleted] = 0 AND [isActive] = 1
                                                         GROUP BY [KPIid]
                                                     )
                                                 ) r ON r.[KPIid] = kpi.[Id]
                                                 WHERE kpi.[isDeleted] = 0
                                                   AND (
                                                       kpi.[Usersid] = @Usersid 
                                                       OR @IsManager = 1 
                                                       OR @IsAdmin = 1
                                                   )
                                                 FOR JSON PATH
                                             ) AS [KPI]

                                      FROM [goals].[vwKPA] kpa
                                      INNER JOIN [Goals].[ContractPillarKPAs] cpk 
                                        ON cpk.[KPAid] = kpa.[Id]
                                        AND cpk.[isDeleted] = 0
                                        AND cpk.[ContractPillarsid] = cp.[id]
                                      WHERE kpa.[isDeleted] = 0
                                        AND kpa.[CompanyUUID] = @CompanyUUID
                                        AND (
                                                       kpa.[Usersid] = @Usersid 
                                                       OR @IsManager = 1 
                                                       OR @IsAdmin = 1
                                                   )
                                      FOR JSON PATH
                                  ) AS [KPA]

                           FROM [Goals].[vwPillars] p
                           LEFT JOIN [Goals].[ContractPillars] cp 
                             ON cp.[Pillarsid] = p.[id]
                             AND cp.[EnterpriseStructureTypesid] = est.[Id]
                             AND cp.[Contractid] = @Contractsid
                             AND cp.[isDeleted] = 0
                           WHERE p.[CompanyUUID] = @CompanyUUID
                             AND p.[isDeleted] = 0
                           ORDER BY p.[OrderVal]
                           FOR JSON PATH
                       ) AS [Pillars]

                FROM [Goals].[vwEnterpriseStructureTypes] est
                WHERE est.[CompanyUUID] = @CompanyUUID
                  AND est.[isDeleted] = 0
                ORDER BY est.[Id]
                FOR JSON PATH
            ) AS EnterpriseStructures

        FROM [Goals].[Contracts] c
        INNER JOIN [Base].[Companies] com ON com.[recordID] = c.[Companyid] AND com.[UUID] = @CompanyUUID
        INNER JOIN [Goals].[ContractPeriods] cp ON cp.[Id] = c.[ContractPeriodsid]
        LEFT JOIN [Base].[Users] u ON u.[Id] = c.[Usersid]
        LEFT JOIN [Goals].[Templates] t ON t.[id] = c.[Templatesid]
        WHERE c.[UUID] = @ContractsUUID
        FOR JSON PATH, ROOT('Data')
    );

    -----------------------------
    -- Return JSON Result
    -----------------------------
    SELECT @json AS JsonResult;

END;
