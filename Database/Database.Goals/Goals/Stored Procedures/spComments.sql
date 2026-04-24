USE [Goals]
GO
/****** Object:  StoredProcedure [admin].[spComments]    Script Date: 09-12-2025 17:32:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [admin].[spComments]
    @CompanyUUID NVARCHAR(200), 
    @TableNameId INT,
    @TableTargetUUID NVARCHAR(200)      -- UUID coming from frontend
AS
BEGIN
    SET NOCOUNT ON;
 
    DECLARE @TableTargetId BIGINT;
 
    -- Convert KPI.UUID → KPI.Id
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
 
    -- If no match found, return empty
    IF @TableTargetId IS NULL
    BEGIN
        SELECT TOP 0 
              [UUID], [CreateDate], [UsersUUID], [FirstName],
              [LastName], [FullName], [IDNumber], [Email],
              [Comment], [CommentTypesid], [CommentTypeName],
              [AttachmentCount]
        FROM [admin].[vwComments];
        RETURN;
    END
 
    -- Fetch comments using numeric TableTargetId
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
    ORDER BY c.[CreateDate] DESC;
 
END