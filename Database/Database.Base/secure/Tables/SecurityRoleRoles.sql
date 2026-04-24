CREATE TABLE [secure].[SecurityRoleRoles] (
    [Id]                   INT IDENTITY (1, 1) NOT NULL,
    [SecurityRolesid]      INT NOT NULL,
    [SecurityRolesidClone] INT NOT NULL,
    [isDeleted]            BIT CONSTRAINT [DF_SecurityRoleRoles_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SecurityRoleRoles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SecurityRoleRoles_SecurityRoles] FOREIGN KEY ([SecurityRolesid]) REFERENCES [secure].[SecurityRoles] ([Id]),
    CONSTRAINT [FK_SecurityRoleRoles_SecurityRoles1] FOREIGN KEY ([SecurityRolesidClone]) REFERENCES [secure].[SecurityRoles] ([Id])
);

