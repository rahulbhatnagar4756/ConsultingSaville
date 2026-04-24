CREATE TABLE [Goals].[ContractPillars] (
    [Id]                         INT             IDENTITY (1, 1) NOT NULL,
    [UUID]                       NVARCHAR (200)  CONSTRAINT [DF_ContractPillars_UUID] DEFAULT (newid()) NOT NULL,
    [Contractid]                 INT             NOT NULL,
    [Pillarsid]                  INT             NOT NULL,
    [EnterpriseStructureTypesid] INT             CONSTRAINT [DF_ContractPillars_EnterpriseStructureTypesid] DEFAULT ((2)) NOT NULL,
    [Weight]                     DECIMAL (18, 4) CONSTRAINT [DF_ContractPillars_Weight] DEFAULT ((100)) NOT NULL,
    [isDeleted]                  BIT             CONSTRAINT [DF_ContractPillars_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ContractPillars] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ContractPillars_Contract] FOREIGN KEY ([Contractid]) REFERENCES [Goals].[Contracts] ([Id]),
    CONSTRAINT [FK_ContractPillars_EnterpriseStructureTypes1] FOREIGN KEY ([EnterpriseStructureTypesid]) REFERENCES [Goals].[EnterpriseStructureTypes] ([Id]),
    CONSTRAINT [FK_ContractPillars_Pillars] FOREIGN KEY ([Pillarsid]) REFERENCES [Goals].[Pillars] ([Id])
);

