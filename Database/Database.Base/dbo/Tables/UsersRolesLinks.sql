CREATE TABLE [dbo].[UsersRolesLinks] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [Companyid]    INT NOT NULL,
    [UsersRolesid] INT NOT NULL,
    [Usersid]      INT NOT NULL,
    [isDeleted]    INT CONSTRAINT [DF_UsersRolesLinks_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UsersRolesLinks] PRIMARY KEY CLUSTERED ([Id] ASC)
);

