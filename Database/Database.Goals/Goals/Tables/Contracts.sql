CREATE TABLE [Goals].[Contracts] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [UUID]                     NVARCHAR (200) CONSTRAINT [DF_Contract_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]                INT            NOT NULL,
    [Usersid]                  INT            NULL,
    [Templatesid]              INT            NULL,
    [ContractPeriodsid]        INT            NULL,
    [DateCreated]              DATETIME       CONSTRAINT [DF_Contract_DateCreated] DEFAULT (getutcdate()) NOT NULL,
    [DateStart]                DATETIME       NOT NULL,
    [DateEnd]                  DATETIME       NULL,
    [isDepartmentTemplateSync] BIT            CONSTRAINT [DF_Contracts_isDepartmentTemplateSync] DEFAULT ((1)) NOT NULL,
    [isIndividualTemplateSync] BIT            CONSTRAINT [DF_Contracts_isIndividualTemplateSync] DEFAULT ((0)) NOT NULL,
    [isActive]                 BIT            CONSTRAINT [DF_Contract_isActive] DEFAULT ((1)) NOT NULL,
    [isDeleted]                BIT            CONSTRAINT [DF_Contract_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Contracts_ContractPeriods] FOREIGN KEY ([ContractPeriodsid]) REFERENCES [Goals].[ContractPeriods] ([Id]),
    CONSTRAINT [FK_Contracts_Templates] FOREIGN KEY ([Templatesid]) REFERENCES [Goals].[Templates] ([Id]) ON DELETE CASCADE
);

