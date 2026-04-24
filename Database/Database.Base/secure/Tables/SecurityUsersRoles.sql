CREATE TABLE [secure].[SecurityUsersRoles] (
    [Id]              INT      IDENTITY (1, 1) NOT NULL,
    [Companyid]       INT      NOT NULL,
    [Usersid]         INT      CONSTRAINT [DF_SecurityPolicys_UsersidCreated] DEFAULT ((1)) NOT NULL,
    [SecurityRolesid] INT      NOT NULL,
    [DateCreated]     DATETIME CONSTRAINT [DF_SecurityPolicys_DateCreated] DEFAULT (getdate()) NOT NULL,
    [isDeleted]       BIT      CONSTRAINT [DF_SecurityUsersRoles_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SecurityPolicys] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SecurityUsersRoles_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID]),
    CONSTRAINT [FK_SecurityUsersRoles_SecurityRoles] FOREIGN KEY ([SecurityRolesid]) REFERENCES [secure].[SecurityRoles] ([Id]),
    CONSTRAINT [FK_SecurityUsersRoles_users] FOREIGN KEY ([Usersid]) REFERENCES [dbo].[users] ([recordid])
);

