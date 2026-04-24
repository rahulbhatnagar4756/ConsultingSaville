




CREATE VIEW [dbo].[vwUserDataFields]
AS
SELECT df.[Recordid]
, l.[CompanyID] 
, df.[UserDataSetID]
, ds.[Name] [UserDataSetsName]
, df.[ControlInputTypesid]
, c.[Name] [ControlInputTypesName]
, df.[LableTypeid]
, l.[Lable]
, l.[LanguageID] 
, df.[LookupTypeid]
, df.[TableNameid]
, t.[Name] [TableNamesName]
, df.[ColumnName]
, df.[TemplateName] 
, df.[Orderval]
, df.[IsRequired]
, df.[isActive] 
, df.[isDeleted]
FROM [iag].[dbo].[UserDataFields] df
INNER JOIN [dbo].[UserDataSets] ds ON ds.[Recordid] = df.[UserDataSetID] 
INNER JOIN [dbo].[Lables] l ON l.[LableTypeID] = df.[LableTypeid] AND l.[isDeleted] = 0 AND l.[isActive] = 1
LEFT OUTER JOIN [dbo].[ControlInputTypes] c ON c.[Recordid] = df.[ControlInputTypesid] 
LEFT OUTER JOIN [dbo].[TableNames] t ON t.[Recordid] = df.[TableNameid] 
