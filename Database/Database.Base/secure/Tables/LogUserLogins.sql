CREATE TABLE [secure].[LogUserLogins] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [Usersid]    BIGINT   NOT NULL,
    [Companyid]  BIGINT   NULL,
    [DateLogged] DATETIME CONSTRAINT [DF_LogUserLogins_DateLogged] DEFAULT (getutcdate()) NOT NULL,
    [Statusid]   INT      NOT NULL,
    CONSTRAINT [PK_LogUserLogins] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LogUserLogins_Status] FOREIGN KEY ([Statusid]) REFERENCES [Base].[Status] ([id])
);

