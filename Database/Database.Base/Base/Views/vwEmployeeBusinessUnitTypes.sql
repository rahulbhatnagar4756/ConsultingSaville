

CREATE VIEW [base].[vwEmployeeBusinessUnitTypes]
AS
SELECT but.[Id]
, but.[UUID]
, but.[Companyid]
, c.[UUID] [CompanyUUID]
, but.[Name]
, but.[Iconsid]
, but.[IconColor]
, ISNULL(i.[Name], 'apartment')  [Icon]
, but.[isDeleted]
FROM [dbo].[EmployeeBusinessUnitTypes] but
LEFT OUTER JOIN [dbo].[Companies] c ON c.[recordID] = but.[Companyid] 
LEFT OUTER JOIN [Admin].[Icons] i ON i.[Id] = but.[Iconsid]
