CREATE TABLE [secure].[SecurityActions] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_SecurityActions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

