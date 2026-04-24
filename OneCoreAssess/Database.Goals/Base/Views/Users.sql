




CREATE VIEW [Base].[Users]
AS
SELECT  u.[recordid] [Id]
      , u.[UUID]
      , u.[type] [UsersTypesid]
      , u.[Username]
      , u.[IDNumber]
      , u.[Title]
      , u.[Firstname]
      , u.[MiddleName] 
      , u.[Lastname]
      , u.[Gender]
      , u.[Race]  
      , u.[Email] 
      , u.[Mobile]
      , u.[EmployeeNumber] 
      , u.[ExternalID]
      , u.[DateOfBirth]
      , DATEDIFF(YEAR, u.DateOfBirth, GETDATE()) 
        - CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, u.DateOfBirth, GETDATE()), u.DateOfBirth) > GETDATE() 
            THEN 1 
            ELSE 0 
        END AS [Age]
      , u.[Seclevel]
      , u.[TandCsigned]
      , u.[TandCsignedDate]
      , u.[isImage]
      , CONVERT(NVARCHAR, u.[recordid]) + 'medium.jpg' [Image]
      , CONVERT(NVARCHAR, u.[recordid]) + 'mini.jpg'   [ImageMini]
      , u.[isActive]  
      , u.[isDeleted]
  FROM [Base].[dbo].[users] u
    

