CREATE TABLE [Goals].[ContractPillarKPAs] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [UUID]              NVARCHAR (200)  CONSTRAINT [DF_PillarKPAs_UUID] DEFAULT (newid()) NOT NULL,
    [ContractPillarsid] INT             NOT NULL,
    [KPAid]             BIGINT          NOT NULL,
    [Weight]            DECIMAL (18, 4) CONSTRAINT [DF_PillarKPAs_Weight] DEFAULT ((100)) NOT NULL,
    [isDeleted]         BIT             CONSTRAINT [DF_PillarKPAs_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PillarKPAs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ContractPillarKPAs_ContractPillars] FOREIGN KEY ([ContractPillarsid]) REFERENCES [Goals].[ContractPillars] ([Id]),
    CONSTRAINT [FK_PillarKPAs_KPA] FOREIGN KEY ([KPAid]) REFERENCES [Goals].[KPA] ([Id])
);

