CREATE TABLE [dbo].[UsersCompanys] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [UUID]           NVARCHAR (200) CONSTRAINT [DF_UsersCompanys_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]      INT            NOT NULL,
    [Usersid]        INT            NOT NULL,
    [UsersidCreated] INT            NOT NULL,
    [DateCreated]    DATETIME       CONSTRAINT [DF_UsersCompanys_DateCreated] DEFAULT (getutcdate()) NOT NULL,
    [DateStarted]    DATE           NULL,
    [EmployeeNo]     NVARCHAR (200) NULL,
    [ContactNumber]  NVARCHAR (30)  NULL,
    [isEmployee]     INT            CONSTRAINT [DF_UsersCompanys_isEmployee] DEFAULT ((0)) NOT NULL,
    [isActive]       INT            CONSTRAINT [DF_UsersCompanys_isActive] DEFAULT ((1)) NOT NULL,
    [isDeleted]      INT            CONSTRAINT [DF_UsersCompanys_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UsersCompanys] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UsersCompanys_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID]),
    CONSTRAINT [FK_UsersCompanys_users] FOREIGN KEY ([Usersid]) REFERENCES [dbo].[users] ([recordid]),
    CONSTRAINT [UK_UsersCompanys] UNIQUE NONCLUSTERED ([Companyid] ASC, [Usersid] ASC)
);

