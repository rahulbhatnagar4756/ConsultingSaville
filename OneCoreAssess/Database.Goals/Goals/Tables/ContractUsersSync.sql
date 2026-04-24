CREATE TABLE [Goals].[ContractUsersSync] (
    [Id]                 INT IDENTITY (1, 1) NOT NULL,
    [Usersid]            INT NOT NULL,
    [Contractid]         INT NOT NULL,
    [ContractidSyncFrom] INT NOT NULL,
    [isDeleted]          BIT CONSTRAINT [DF_ContractUsersSync_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ContractUsersSync] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ContractUsersSync_Contracts] FOREIGN KEY ([ContractidSyncFrom]) REFERENCES [Goals].[Contracts] ([Id]),
    CONSTRAINT [FK_ContractUsersSync_Contracts1] FOREIGN KEY ([Contractid]) REFERENCES [Goals].[Contracts] ([Id])
);

