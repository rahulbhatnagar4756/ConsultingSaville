CREATE TABLE [dbo].[UsersRoles] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [UUID]      NVARCHAR (200) CONSTRAINT [DF_UsersRoles_UUID] DEFAULT (newid()) NOT NULL,
    [Name]      NVARCHAR (200) NOT NULL,
    [isDeleted] INT            CONSTRAINT [DF_UsersRoles_isdeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UsersRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

