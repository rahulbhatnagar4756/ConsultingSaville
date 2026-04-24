USE [Goals]
GO
/****** Object:  StoredProcedure [admin].[spCommentAttachments_GetByUUID]    Script Date: 09/12/2025 15:17:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE OR ALTER   PROCEDURE [admin].[spCommentAttachments_GetByUUID]
    @UUID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
 
    SELECT 
        [Id],
        [UUID],
        [CommentsId],
        [CreatedDate],
        [FileName],
        [FilePath],
        [FileSize],
        [MimeType],
        [isDeleted]
    FROM [admin].[CommentAttachments]
    WHERE [UUID] = @UUID
      AND [isDeleted] = 0;
END