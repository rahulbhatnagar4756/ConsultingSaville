


--create a query that links all foreign keys for table EmployeesCorporateAccessLevels
CREATE VIEW [base].[vwEmployeesCorporateAccessLevels] 
AS
SELECT ecal.[Id]
, ecal.[UUID]
, ecal.[Companyid]
, c.[UUID] AS [CompanyUUID]
, ecal.[Usersid]
, u.[UUID] AS [UserUUID]
, u.[firstname] + ' ' + u.[lastname] [FullName]
, u.[IDNumber]
, u.[EmployeeNumber]
, ecal.[EmployeeBusinessUnitTypesid]
, ebut.[UUID] AS [EmployeeBusinessUnitTypeUUID]
, ebut.[Name] AS [EmployeeBusinessUnitType]
, ecal.[EmployeeBusinessUnitsid]
, ebu.[UUID] AS [EmployeeBusinessUnitUUID]
, ebu.[Name] AS [EmployeeBusinessUnit]
, ecal.[EmployeeDepartmentsid]
, ed.[UUID] AS [EmployeeDepartmentUUID]
, ed.[Name] AS [EmployeeDepartment]
, ecal.[isFullAccess]
, ecal.[isActive]
, ecal.[isDeleted]
FROM [base].[EmployeesCorporateAccessLevels] ecal
INNER JOIN [dbo].[Companies] c ON ecal.Companyid = c.recordID
LEFT OUTER JOIN [dbo].[EmployeeBusinessUnits] ebu ON ecal.EmployeeBusinessUnitsid = ebu.Recordid
LEFT OUTER JOIN [dbo].[EmployeeBusinessUnitTypes] ebut ON ecal.EmployeeBusinessUnitTypesid = ebut.Id
LEFT OUTER JOIN [dbo].[EmployeeDepartments] ed ON ecal.EmployeeDepartmentsid = ed.Recordid
INNER JOIN [dbo].[users] u ON ecal.Usersid = u.recordid
WHERE ecal.isActive = 1 
	AND ecal.isDeleted = 0;
 
