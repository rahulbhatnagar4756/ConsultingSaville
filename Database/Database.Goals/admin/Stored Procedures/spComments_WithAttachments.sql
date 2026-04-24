

CREATE PROCEDURE [admin].[spComments_WithAttachments]
	@CompanyUUID NVARCHAR(200), 
	@TableNameid INT,
	@TableTargetid BIGINT
AS
BEGIN

	SELECT c.[UUID]
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

	, ( SELECT att.[FileName]
		, att.[FilePath]
		, att.[FileSize]
		, att.[MimeType]
		FROM [Admin].[CommentAttachments] att
		WHERE att.[CommentsId] = c.[id]
			AND att.[isDeleted] = 0
		FOR JSON PATH			
			) AS [Attachments]

	FROM [admin].[vwComments] c
	WHERE c.[isDeleted] = 0 
		AND c.[CompanyUUID] = @CompanyUUID
		AND c.[TableNameid] = @TableNameid
		AND c.[TableTargetid] = @TableTargetid
	ORDER BY c.[CreateDate]	
	FOR JSON PATH, ROOT('Data')

END