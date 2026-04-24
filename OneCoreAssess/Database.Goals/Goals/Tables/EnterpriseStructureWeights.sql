CREATE TABLE [Goals].[EnterpriseStructureWeights] (
    [Id]                         INT             IDENTITY (1, 1) NOT NULL,
    [UUID]                       NVARCHAR (200)  CONSTRAINT [DF_EnterpriseStructureWeights_UUID] DEFAULT (newid()) NOT NULL,
    [EnterpriseStructureTypesid] INT             NOT NULL,
    [EmployeeLevelsid]           INT             NOT NULL,
    [Weight]                     DECIMAL (18, 4) NOT NULL,
    [isDeleted]                  BIT             CONSTRAINT [DF_EnterpriseStructureWeights_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EnterpriseStructureWeights] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EnterpriseStructureWeights_EnterpriseStructureTypes] FOREIGN KEY ([EnterpriseStructureTypesid]) REFERENCES [Goals].[EnterpriseStructureTypes] ([Id])
);

