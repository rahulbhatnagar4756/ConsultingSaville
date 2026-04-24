CREATE TABLE [secure].[SecurityCompanyRoles] (
    [Id]              INT IDENTITY (1, 1) NOT NULL,
    [Companyid]       INT NOT NULL,
    [SecurityRolesid] INT NOT NULL,
    [isDeleted]       BIT CONSTRAINT [DF_SecurityCompanyRoles_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SecurityCompanyRoles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SecurityCompanyRoles_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID]),
    CONSTRAINT [FK_SecurityCompanyRoles_SecurityRoles] FOREIGN KEY ([SecurityRolesid]) REFERENCES [secure].[SecurityRoles] ([Id])
);

