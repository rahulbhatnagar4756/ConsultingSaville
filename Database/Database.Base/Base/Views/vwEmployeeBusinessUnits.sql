





CREATE VIEW [base].[vwEmployeeBusinessUnits]
AS
SELECT  bu.[Recordid] [Id]
, bu.[UUID]
, bu.[Companyid]
, c.[UUID] [CompanyUUID]
, bu.[EmployeeBusinessUnitTypesid]
, but.[UUID] [EmployeeBusinessUnitTypesUUID]
, but.[Name] [EmployeeBusinessUnitTypes]
, bu.[Name]
, bu.[Description]
, ISNULL(bu.[IconId], but.[Iconsid]) [Iconsid]
, ISNULL(i.[Name], 'apartment')  [Icon]
, ISNULL(ISNULL(bu.[IconColor], but.[IconColor]), '#362f21') [IconColor]
, bu.[isDeleted]
FROM [dbo].[EmployeeBusinessUnits] bu
LEFT OUTER JOIN [dbo].[EmployeeBusinessUnitTypes] but ON but.[Id] = bu.[EmployeeBusinessUnitTypesid]
LEFT OUTER JOIN [dbo].[Companies] c ON c.[recordID] = ISNULL(bu.[companyid], but.[Companyid])
LEFT OUTER JOIN [Admin].[Icons] i ON i.[Id] = ISNULL(bu.[IconId], but.[Iconsid])

