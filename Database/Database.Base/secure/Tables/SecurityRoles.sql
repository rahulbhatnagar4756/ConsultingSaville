CREATE TABLE [secure].[SecurityRoles] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (200) NOT NULL,
    [NameFriendly] NVARCHAR (200) NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [isMainRole]   BIT            CONSTRAINT [DF_SecurityRoles_isMainRole] DEFAULT ((0)) NOT NULL,
    [isDeleted]    INT            CONSTRAINT [DF_SecurityRoles_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SecurityRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

