




CREATE VIEW [Base].[vwEmployeeDepartments]
AS
SELECT e.[Recordid] [Id]
      ,e.[UUID]
      ,e.[Companyid]
      ,c.[UUID] [CompanyUUID]
      ,c.[Name] [Company]
      ,e.[EmployeeBusinessUnitsid]
      , b.[UUID] [EmployeeBusinessUnitsUUID]
      , b.[Name] [EmployeeBusinessUnits]
      ,e.[Name]
      ,e.[Description]
      ,e.[IconId]
      ,e.[IconColor]
      ,e.[isDeleted]
  FROM [Base].[dbo].[EmployeeDepartments] e
  INNER JOIN [Base].[Companies] c ON c.[recordID] = e.[Companyid] 
  LEFT OUTER JOIN [Base].[vwEmployeeBusinessUnits] b ON b.[Id] = e.[EmployeeBusinessUnitsid]
