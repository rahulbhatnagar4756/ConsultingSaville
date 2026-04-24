








CREATE VIEW [Goals].[vwKPI]
AS
SELECT  kpi.[Id]
, kpi.[UUID]
, kpi.[Companyid]
, c.[UUID] [CompanyUUID]
, c.[Name] [Company]
, kpi.[Usersid]
, u.[UUID] [UsersUUID]
, u.[Firstname]
, u.[Lastname]
, u.[IDNumber]
, u.[Email]
, u.[EmployeeNumber] 
, kpi.[Statusid]
, s.[UUID] [StatusUUID]
, s.[Name] [Status]
, kpi.[ToleranceSetsid]
, t.[UUID] [ToleranceSetsUUID]
, t.[Name] [ToleranceSets]
, t.[Description] [ToleranceSetsDescription]
, kpi.[DateCreated]
, kpi.[Name]
, kpi.[Description]
, kpi.[DateStart]
, kpi.[DateEnd]
, kpi.[Target]
, kpi.[isScoreProcessing]
, kpi.[isActive]
, kpi.[isDeleted]
FROM [Goals].[KPI] kpi
INNER JOIN [Base].[Companies] c ON c.[recordID] = kpi.[Companyid] 
INNER JOIN [Base].[Users] u ON u.[Id] = kpi.[Usersid]
LEFT OUTER JOIN [Goals].[Status] s ON s.[Id] = kpi.[Statusid] 
LEFT OUTER JOIN [Goals].[ToleranceSets] t ON t.[Id] = kpi.[ToleranceSetsid]