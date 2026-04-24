


CREATE VIEW [admin].[vwComments]
AS
SELECT
    c.[Id],
    c.[UUID],
    c.[CreateDate],
    c.[Comment],
    c.[isDeleted],

    -- User
    u.[Id] AS [Usersid],
    u.[UUID] [UsersUUID],
    u.[FirstName],
    u.[LastName],
    (u.[FirstName] + ISNULL(' ' + u.[LastName], '')) AS [FullName],
    u.[IDNumber],
    u.[Email],
    -- Company
    comp.[Recordid] AS [Companyid],
    comp.[UUID] [CompanyUUID],
    comp.[Name] AS [CompanyName],

    -- Table Name
    tn.[Id] AS [TableNameid],
    tn.[Name] AS [TableName],

    -- Row ID on target table
    c.[TableTargetid],

    -- Comment type
    ct.[Id] AS [CommentTypesid],
    ct.[Name] AS [CommentTypeName],

    -- Attachment count
    ISNULL(att.[AttachmentCount], 0) AS [AttachmentCount]

FROM [admin].[Comments] AS c
LEFT JOIN [admin].[CommentTypes] AS ct ON c.[CommentTypesid] = ct.[Id]
LEFT JOIN [Goals].[TableName] AS tn ON c.[TableNameid] = tn.[Id]
LEFT JOIN [Base].[Users] AS u ON c.[Usersid] = u.[Id]
LEFT JOIN [Base].[Companies] AS comp ON comp.[recordID] = c.[Companyid] 

LEFT JOIN (
        SELECT 
            [Commentsid],
            COUNT(*) AS [AttachmentCount]
        FROM [admin].[CommentAttachments]
        WHERE [isDeleted] = 0
        GROUP BY [Commentsid]
    ) AS [att]
        ON [att].[Commentsid] = [c].[Id];