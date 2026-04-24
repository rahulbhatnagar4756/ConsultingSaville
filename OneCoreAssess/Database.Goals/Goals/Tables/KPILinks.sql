CREATE TABLE [Goals].[KPILinks] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [UUID]          NVARCHAR (200)  CONSTRAINT [DF_KPILinks_UUID] DEFAULT (newid()) NULL,
    [KPIId]         INT             NOT NULL,
    [KPIIdLinkedTo] INT             NOT NULL,
    [Weight]        DECIMAL (18, 4) CONSTRAINT [DF_KPILinks_Weight] DEFAULT ((100)) NOT NULL,
    [isDeleted]     BIT             NOT NULL,
    CONSTRAINT [PK_KPILinks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_KPILinks_KPI2] FOREIGN KEY ([KPIId]) REFERENCES [Goals].[KPI] ([Id]),
    CONSTRAINT [FK_KPILinks_KPI3] FOREIGN KEY ([KPIIdLinkedTo]) REFERENCES [Goals].[KPI] ([Id])
);

