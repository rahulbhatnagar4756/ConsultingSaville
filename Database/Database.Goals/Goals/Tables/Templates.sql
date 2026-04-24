CREATE TABLE [Goals].[Templates] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [UUID]           NVARCHAR (200) CONSTRAINT [DF_Templates_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]      INT            NOT NULL,
    [DateCreated]    DATETIME       CONSTRAINT [DF_Templates_DateCreated] DEFAULT (getutcdate()) NOT NULL,
    [UsersIdCreated] INT            NULL,
    [Name]           NVARCHAR (500) NOT NULL,
    [Code]           NVARCHAR (500) NULL,
    [isDeleted]      BIT            CONSTRAINT [DF_Templates_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Templates] PRIMARY KEY CLUSTERED ([Id] ASC)
);

