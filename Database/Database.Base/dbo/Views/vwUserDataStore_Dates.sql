




CREATE VIEW [dbo].[vwUserDataStore_Dates]
as
SELECT d.*
, DATEDIFF(YEAR, d.[date], GETDATE()) - CASE WHEN DATEFROMPARTS(YEAR(GETDATE()), MONTH(d.[date]), CASE WHEN MONTH(d.[date]) = 2 AND DAY(d.[date]) = 29 THEN 28 ELSE DAY(d.[date]) END) < GETDATE() THEN 0 ELSE 1 END [Years],
    DATEDIFF(MONTH, d.[date], GETDATE()) % 12 AS [Months]
FROM (SELECT d.[Recordid], d.[Userid], d.[UserDataFieldsID] [Typeid]
	  , CASE  d.[UserDataFieldsID] WHEN 76 THEN 'Birthday'
	  							 WHEN 80 THEN 'Company Start Date'
	  							 END [TypeName]
	  , d.[txtValue] [OriginalDate]
	  , CASE WHEN ISDATE(d.[txtValue]) = 1 THEN CAST(d.[txtValue] as date) END [Date]
	  FROM [UserDataStore] d
	  WHERE d.[UserDataFieldsID] in (76,80) AND (LEN(RTRIM(LTRIM(d.[txtValue]))) > 3) AND d.[isDeleted] = 0) d

	 
