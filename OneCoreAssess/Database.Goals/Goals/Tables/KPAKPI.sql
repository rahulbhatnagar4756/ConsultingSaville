CREATE TABLE [Goals].[KPAKPI] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [UUID]      NVARCHAR (200)  NOT NULL,
    [KPAid]     INT             NOT NULL,
    [KPIid]     INT             NOT NULL,
    [Weight]    DECIMAL (18, 4) CONSTRAINT [DF_KPAKPI_Weight] DEFAULT ((100)) NOT NULL,
    [isDeleted] BIT             CONSTRAINT [DF_KPAKPI_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_KPAKPI] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_KPAKPI_KPA] FOREIGN KEY ([KPAid]) REFERENCES [Goals].[KPA] ([Id]),
    CONSTRAINT [FK_KPAKPI_KPI] FOREIGN KEY ([KPIid]) REFERENCES [Goals].[KPI] ([Id])
);

