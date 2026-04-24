CREATE TABLE [admin].[CommentTypes] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (255) NOT NULL,
    [isDeleted] BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CommentTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

