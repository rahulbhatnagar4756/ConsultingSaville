




CREATE VIEW [base].[vwEmployeeDepartments]
AS
SELECT d.[Recordid] [Id]
      , d.[UUID]
      , d.[Companyid]
      , c.[UUID] [CompanyUUID]
      , bu.EmployeeBusinessUnitTypesid
      , bu.[EmployeeBusinessUnitTypesUUID]
      , bu.[EmployeeBusinessUnitTypes]
      , d.[EmployeeBusinessUnitsid]
      , bu.[UUID] [EmployeeBusinessUnitsUUID]
      , bu.[Name] [EmployeeBusinessUnits]
      , d.[Name]
      , d.[Description]
      , ISNULL(d.[IconId], bu.[Iconsid]) [Iconsid]
      , ISNULL(i.[Name], 'apartment')  [Icon]
      , ISNULL(d.[IconColor], bu.[IconColor]) [IconColor]
      , d.[isDeleted]
  FROM [dbo].[EmployeeDepartments] d
  INNER JOIN [dbo].[Companies] c ON c.[recordID] = d.[Companyid]
  LEFT OUTER JOIN [base].[vwEmployeeBusinessUnits] bu ON bu.[Id] = d.[EmployeeBusinessUnitsid] 
  LEFT OUTER JOIN [Admin].[Icons] i ON i.[Id] = ISNULL(d.[IconId], bu.[Iconsid])
