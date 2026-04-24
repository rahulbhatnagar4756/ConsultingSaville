CREATE TABLE [Goals].[UserTemplates] (
    [Id]                INT  PRIMARY KEY IDENTITY(1,1)     NOT NULL,
    [Templatesid]       INT      NOT NULL,
    [Usersid]           BIGINT   NOT NULL,
    [ContractPeriodsid] INT      NOT NULL,
    [DateCreated]       DATETIME CONSTRAINT [DF_UserTemplates_DateCreated] DEFAULT (getutcdate()) NOT NULL,
    [isDeleted]         BIT      CONSTRAINT [DF_UserTemplates_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [FK_UserTemplates_ContractPeriods] FOREIGN KEY ([ContractPeriodsid]) REFERENCES [Goals].[ContractPeriods] ([Id]),
    CONSTRAINT [FK_UserTemplates_Templates] FOREIGN KEY ([Templatesid]) REFERENCES [Goals].[Templates] ([Id])
);

