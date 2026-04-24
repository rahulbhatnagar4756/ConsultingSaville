CREATE TABLE [Goals].[ContractEnterpriseStructureTypes] (
    [Id]                         INT             IDENTITY (1, 1) NOT NULL,
    [Contractsid]                INT             NOT NULL,
    [EnterpriseStructureTypesid] INT             NOT NULL,
    [Weight]                     DECIMAL (18, 2) CONSTRAINT [DF_ContractEnterpriseStructureTypes_Weight] DEFAULT ((100)) NOT NULL,
    [isDeleted]                  BIT             CONSTRAINT [DF_ContractEnterpriseStructureTypes_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ContractEnterpriseStructureTypes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ContractEnterpriseStructureTypes_Contracts] FOREIGN KEY ([Contractsid]) REFERENCES [Goals].[Contracts] ([Id]),
    CONSTRAINT [FK_ContractEnterpriseStructureTypes_EnterpriseStructureTypes] FOREIGN KEY ([EnterpriseStructureTypesid]) REFERENCES [Goals].[EnterpriseStructureTypes] ([Id])
);

