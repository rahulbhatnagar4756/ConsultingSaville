
CREATE VIEW [Base].[EmployeeHierarchy]
AS
SELECT ROW_NUMBER() OVER(PARTITION BY [Usersid] ORDER BY [CreateDate] DESC) [No]
, [Recordid] [Id]
, [UUID]
, [EmployeeJobsid]
, [Usersid]
, [UsersidManager]
, [CreateDate]
, [isDeleted]
FROM [Base].[dbo].[EmployeeHierarchy]
WHERE [isDeleted] = 0
