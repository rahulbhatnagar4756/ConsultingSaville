CREATE TABLE [admin].[Comments] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [UUID]           NVARCHAR (200) CONSTRAINT [DF_Comments_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]      BIGINT         NOT NULL,
    [Usersid]        BIGINT         NOT NULL,
    [TableNameid]    INT            NOT NULL,
    [TableTargetid]  BIGINT         NOT NULL,
    [CreateDate]     DATETIME       CONSTRAINT [DF__Comments__Create__39987BE6] DEFAULT (getutcdate()) NOT NULL,
    [Comment]        NVARCHAR (MAX) NOT NULL,
    [CommentTypesid] INT            CONSTRAINT [DF_Comments_CommentTypesid] DEFAULT ((1)) NULL,
    [isDeleted]      BIT            CONSTRAINT [DF__Comments__isDele__3A8CA01F] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Comments_CommentTypes] FOREIGN KEY ([CommentTypesid]) REFERENCES [admin].[CommentTypes] ([Id]),
    CONSTRAINT [FK_Comments_TableName] FOREIGN KEY ([TableNameid]) REFERENCES [Goals].[TableName] ([Id])
);

