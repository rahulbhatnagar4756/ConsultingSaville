CREATE TABLE [admin].[CommentAttachments] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [UUID]        NVARCHAR (200) CONSTRAINT [DF_CommentAttachments_UUID] DEFAULT (newid()) NOT NULL,
    [CommentsId]  BIGINT         NOT NULL,
    [CreatedDate] DATETIME2 (7)  CONSTRAINT [DF__CommentAt__Creat__44160A59] DEFAULT (getutcdate()) NOT NULL,
    [FileName]    NVARCHAR (255) NOT NULL,
    [FilePath]    NVARCHAR (500) NOT NULL,
    [FileSize]    INT            NULL,
    [MimeType]    NVARCHAR (100) NULL,
    [isDeleted]   BIT            CONSTRAINT [DF__CommentAt__isDel__450A2E92] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__CommentA__3214EC07D0F5BABD] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CommentAttachments_Comments] FOREIGN KEY ([CommentsId]) REFERENCES [admin].[Comments] ([Id])
);

