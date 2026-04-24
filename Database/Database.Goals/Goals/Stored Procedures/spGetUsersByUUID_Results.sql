USE [Goals]
GO
/****** Object:  StoredProcedure [Goals].[spGetUsersByUUID_Results]    Script Date: 23/12/2025 15:43:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [Goals].[spGetUsersByUUID_Results]
    @UsersUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @json NVARCHAR(MAX);
    DECLARE @HistoryJSON NVARCHAR(MAX);
    DECLARE @KPAANDKPIJSON NVARCHAR(MAX);
    DECLARE @TEAMMEMBERS NVARCHAR(MAX);
    DECLARE @Usersid INT;
    DECLARE @Name NVARCHAR(100);


    SELECT @Usersid = recordid, @Name = firstname +' '+ lastname 
    FROM [BASE].[dbo].[Users]
    WHERE [UUID] = @UsersUUID;


    -- Build dynamic History JSON
    SELECT @HistoryJSON = ISNULL(
        (SELECT 
            eh.CreateDate AS CreateDate, 
            eh.BusinessUnit AS BusinessUnit, 
            eh.Department AS Department , 
            eh.Position AS Position, 
            el.Name AS Levels
         FROM [Base].[dbo].[vwEmployeeHierarchy] eh
         INNER JOIN [Base].[dbo].[EmployeeLevels] el 
             ON el.Id = eh.EmployeeLevelsid
         WHERE eh.Usersid = @Usersid
         FOR JSON PATH
        ), ''
    );

     WITH UserKPIs AS (
        SELECT DISTINCT 
            kpi.Id AS KPIId,
            kpi.Name AS KPIName
        FROM [Goals].[vwKPI] kpi
        WHERE kpi.isDeleted = 0
          AND kpi.Usersid = @Usersid
    ),

    /* ============================================
       STEP 2: Latest KPI result per quarter
    ============================================ */
    KPI_QuarterResults AS (
        SELECT
            r.KPIid,
            DATEPART(QUARTER, r.DateAdded) AS QuarterNo,
            r.Result,
            ROW_NUMBER() OVER (
                PARTITION BY r.KPIid, DATEPART(QUARTER, r.DateAdded)
                ORDER BY r.DateAdded DESC
            ) AS rn
        FROM [Goals].[Results] r
        INNER JOIN UserKPIs uk 
            ON uk.KPIId = r.KPIid
        WHERE r.isDeleted = 0
          AND r.isActive = 1
    ),

    /* ============================================
       STEP 3: Pivot quarterly results
    ============================================ */
    KPI_Pivot AS (
        SELECT
            KPIid,
            SUM(CASE WHEN QuarterNo = 1 THEN Result ELSE 0 END) AS Q1,
            SUM(CASE WHEN QuarterNo = 2 THEN Result ELSE 0 END) AS Q2,
            SUM(CASE WHEN QuarterNo = 3 THEN Result ELSE 0 END) AS Q3,
            SUM(CASE WHEN QuarterNo = 4 THEN Result ELSE 0 END) AS Q4
        FROM KPI_QuarterResults
        WHERE rn = 1
        GROUP BY KPIid
    ),

    /* ============================================
       STEP 4: KPI rows
    ============================================ */
    KPI_ROWS AS (
        SELECT
            p.Name AS Pillar,
            kpa.Id AS KPAId,
            kpa.Name AS KPAName,
            kpi.Id AS KPIId,
            'KPI' AS [Type],
            kpi.Name AS [Name],
            ISNULL(kp.Q1, 0) AS Q1,
            ISNULL(kp.Q2, 0) AS Q2,
            ISNULL(kp.Q3, 0) AS Q3,
            ISNULL(kp.Q4, 0) AS Q4
        FROM UserKPIs uk
        INNER JOIN [Goals].[vwKPI] kpi
            ON kpi.Id = uk.KPIId
        INNER JOIN [Goals].[KPAKPI] lk
            ON lk.KPIid = kpi.Id AND lk.isDeleted = 0
        INNER JOIN [Goals].[vwKPA] kpa
            ON kpa.Id = lk.KPAid AND kpa.isDeleted = 0
        INNER JOIN [Goals].[ContractPillarKPAs] cpk
            ON cpk.KPAid = kpa.Id AND cpk.isDeleted = 0
        INNER JOIN [Goals].[ContractPillars] cp
            ON cp.Id = cpk.ContractPillarsid AND cp.isDeleted = 0
        INNER JOIN [Goals].[vwPillars] p
            ON p.Id = cp.Pillarsid AND p.isDeleted = 0
        LEFT JOIN KPI_Pivot kp
            ON kp.KPIid = kpi.Id
    ),

    /* ============================================
       STEP 5: KPA rows (aggregated from KPIs)
    ============================================ */
    KPA_ROWS AS (
        SELECT
            Pillar,
            KPAId,
            KPAName,
            NULL AS KPIId,
            'KPA' AS [KpaKpi],
            KPAName AS [Name],
            SUM(Q1) AS Q1,
            SUM(Q2) AS Q2,
            SUM(Q3) AS Q3,
            SUM(Q4) AS Q4
        FROM KPI_ROWS
        GROUP BY Pillar, KPAId, KPAName
    )

    /* ============================================
       FINAL OUTPUT
    ============================================ */
     SELECT @KPAANDKPIJSON = ISNULL(
        (SELECT
        Pillar,
        [KpaKpi],
        [Name],
        Q1,
        Q2,
        Q3,
        Q4
    FROM (
        SELECT * FROM KPA_ROWS
        UNION ALL
        SELECT * FROM KPI_ROWS
    ) x
    ORDER BY
        Pillar,
        KPAId,
        CASE WHEN [KpaKpi] = 'KPA' THEN 0 ELSE 1 END,
        KPIId
        FOR JSON PATH
        ), ''
    );

    SELECT @TEAMMEMBERS = ISNULL(
     (
         SELECT  eh.FirstName + ' ' + eh.LastName AS [Name],
             '' AS ImageBase64,
             rs.Score AS Score,
             eh.Position AS [Role]
         FROM [Base].[dbo].[vwEmployeeHierarchy] AS eh inner join [Goals].[KPI] AS kpi on kpi.Usersid=eh.Usersid left join [Goals].[Results]
         AS rs on rs.KPIid=kpi.Id
         WHERE eh.UsersIdManager = @Usersid
         FOR JSON PATH
     ), '[]'
     );
     



    -- Construct the full JSON (keep all existing hardcoded structure)
    SET @json = '{
        "Name": "'+@Name+'",
        "CompanyLogo": "https://example.com/logo.png",
        "TimePeriod": "2025 Q1-Q4",
        "Quarters": [85, 90, 88, 92, 88],
        "History": ' + @HistoryJSON + ',
        "Pillars": ["Overview", "Business Processes", "ContinuousImprovement","Customer","Organisational L&D","SHEQ"],
        "KpaKpis": ' + @KPAANDKPIJSON + ',
        "TeamMembers":' + @TEAMMEMBERS + '
    }';

    -- Remove unwanted escape characters (\r, \n)
    SET @json = REPLACE(@json, CHAR(13), '');
    SET @json = REPLACE(@json, CHAR(10), '');

    -- Return JSON wrapped in a list
    SELECT '{"Data":[' + @json + ']}' AS JsonResult;
END
