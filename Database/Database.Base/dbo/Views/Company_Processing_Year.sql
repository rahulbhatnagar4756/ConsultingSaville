


CREATE VIEW [dbo].[Company_Processing_Year]
AS
SELECT c.[recordID]
, c.[UUID]
, ISNULL(s2.[Value], s.[Value]) [ProcessingYear] 
FROM [dbo].[Companies] c
INNER JOIN [dbo].[CompanySettings] s ON s.[Companyid] = 1 AND s.[CompanySettingTypesid] = 27 AND s.[isDeleted] = 0
LEFT OUTER JOIN [dbo].[CompanySettings] s2 ON s2.[Companyid] = c.[recordID] AND s2.[CompanySettingTypesid] = 27 AND s2.[isDeleted] = 0
