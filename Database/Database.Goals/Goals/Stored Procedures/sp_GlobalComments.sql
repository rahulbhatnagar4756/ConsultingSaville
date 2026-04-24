USE [Goals]
GO
/****** Object:  StoredProcedure [admin].[sp_GlobalComments]    Script Date: 09-12-2025 16:57:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
ALTER     PROCEDURE [admin].[sp_GlobalComments]
    @CompanyUUID NVARCHAR(200), 
    @TableNameId INT,    
    @TableTargetUUID NVARCHAR(200),
    @UsersUUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
 
    DECLARE @TableTargetId BIGINT;
    DECLARE @IsManager BIT = 0;
    DECLARE @IsAdmin BIT = 0;
    DECLARE @IsManagerOrAdmin BIT = 0;
 
    ---------------------------------------------------------
    -- 1) Convert TableTargetUUID → TableTargetId
    ---------------------------------------------------------
    SELECT @TableTargetId = kpi.Id
    FROM [Goals].[Goals].[KPI] kpi
    WHERE kpi.UUID = @TableTargetUUID
      AND kpi.isDeleted = 0;
 
  -- If not found in KPI, look in KPA table
IF @TableTargetId IS NULL
BEGIN
    SELECT @TableTargetId = kpa.Id
    FROM [Goals].[Goals].[KPA] kpa
    WHERE kpa.UUID = @TableTargetUUID
      AND kpa.isDeleted = 0;
END
 
    ---------------------------------------------------------
    -- 2) Is Manager?
    ---------------------------------------------------------
    SELECT 
        @IsManager = CASE 
                        WHEN EXISTS (
                            SELECT 1
                            FROM [Goals].[Base].[vwEmployees]
                            WHERE UsersUUIDManager = @UsersUUID
                        ) THEN 1 ELSE 0 END;
 
 
    ---------------------------------------------------------
    -- 3) Is Admin?
    ---------------------------------------------------------
    SELECT 
        @IsAdmin = CASE WHEN EXISTS (
            SELECT 1
            FROM  [BASE].[dbo].[UsersRoles] r
            INNER JOIN [BASE].[dbo].[UsersRolesLinks] rl 
                ON rl.[UsersRolesid] = r.[Id] 
               AND rl.[isDeleted] = 0
            INNER JOIN [BASE].[dbo].[users] u 
                ON u.[recordid] = rl.[Usersid] 
               AND u.[UUID] = @UsersUUID
            INNER JOIN [BASE].[dbo].[Companies] c 
                ON c.[recordID] = rl.[Companyid] 
               AND c.[UUID] = @CompanyUUID
            WHERE r.[isDeleted] = 0
              AND r.Name IN ('AdminGoals')
        ) THEN 1 ELSE 0 END;
 
    SET @IsManagerOrAdmin = CASE WHEN @IsManager = 1 OR @IsAdmin = 1 THEN 1 ELSE 0 END;
 
 
    ---------------------------------------------------------
    -- 4) Build JSON Output
    ---------------------------------------------------------
    IF @IsManagerOrAdmin = 0
    BEGIN
        SELECT 
            (
                SELECT 
                    @IsManagerOrAdmin AS isManagerOrAdmin,
                    NULL AS comments
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            ) AS JsonResult;
        RETURN;
    END
 
 
    ---------------------------------------------------------
    -- 5) Return JSON including comments list
    ---------------------------------------------------------
    SELECT 
        (
            SELECT
                @IsManagerOrAdmin AS isManagerOrAdmin,
                (
                    SELECT                         
                          c.[UUID]
                        , c.[CreateDate]
                        , c.[UsersUUID]
                        , c.[FirstName]
                        , c.[LastName]
                        , c.[FullName]
                        , c.[IDNumber]
                        , c.[Email]
                        , c.[Comment]
                        , c.[CommentTypesid]
                        , c.[CommentTypeName]
                        , c.[AttachmentCount]
                        ,ca.[UUID] AS AttachmentsUUID
                        ,ca.[FileName]
                        ,ca.[CreatedDate] AS FileCreatedDate
                        ,ca.[FileSize]
                        ,ca.[FilePath]
                    FROM [admin].[vwComments] c LEFT JOIN [admin].[CommentAttachments] AS ca ON ca.CommentsId = c.[Id]
                    WHERE c.[isDeleted] = 0 
                      AND c.[CompanyUUID] = @CompanyUUID
                      AND c.[TableNameid] = @TableNameId   
                      AND c.[TableTargetid] = @TableTargetId
                    ORDER BY c.[CreateDate] DESC
                    FOR JSON PATH
                ) AS comments
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) AS JsonResult;
 
END