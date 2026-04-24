
CREATE VIEW [Base].[vwEmployeeBusinessUnits]
AS
SELECT  e.[Recordid] [Id]
      , e.[UUID]
      , e.[Companyid]
      , c.[UUID] [CompanyUUID]
      , c.[Name] [Company]
      , e.[EmployeeBusinessUnitTypesid]
      , e.[Name]
      , e.[Description]
      , e.[IconId]
      , e.[IconColor]
      , e.[isDeleted]
  FROM [Base].[dbo].[EmployeeBusinessUnits] e
  INNER JOIN [Base].[Companies] c ON c.[recordID] = e.[Companyid] 
