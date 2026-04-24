CREATE TABLE [secure].[SecurityRoleActions] (
    [Id]                INT IDENTITY (1, 1) NOT NULL,
    [SecurityRolesid]   INT NOT NULL,
    [SecurityActionsid] INT NOT NULL,
    [isDeleted]         BIT CONSTRAINT [DF_SecurityRoleActions_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SecurityRoleActions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SecurityRoleActions_SecurityActions] FOREIGN KEY ([SecurityActionsid]) REFERENCES [secure].[SecurityActions] ([Id]),
    CONSTRAINT [FK_SecurityRoleActions_SecurityRoles] FOREIGN KEY ([SecurityRolesid]) REFERENCES [secure].[SecurityRoles] ([Id])
);

