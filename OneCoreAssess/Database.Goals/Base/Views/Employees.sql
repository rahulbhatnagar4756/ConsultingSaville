


CREATE VIEW [Base].[Employees]
as
SELECT [Recordid] [Id]
, [Userid] [Usersid]
, [Companyid]
, [DateCreated]
, [CreatedBy]
, [DateUpdated]
, [UpdatedBy]
, [isdeleted]
, [isActive]
FROM [Base].[dbo].[Employees] 
WHERE [isdeleted] = 0

