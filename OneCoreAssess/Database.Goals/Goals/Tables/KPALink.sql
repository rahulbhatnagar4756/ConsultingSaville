CREATE TABLE [Goals].[KPALink] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [UUID]        NVARCHAR (200)  NOT NULL,
    [KPA]         INT             NOT NULL,
    [KPALinkedTo] INT             NOT NULL,
    [Weight]      DECIMAL (18, 4) CONSTRAINT [DF_KPALink_Weight] DEFAULT ((100)) NOT NULL,
    [isDeleted]   BIT             CONSTRAINT [DF_KPALink_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_KPALink] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_KPALink_KPA] FOREIGN KEY ([KPA]) REFERENCES [Goals].[KPA] ([Id]),
    CONSTRAINT [FK_KPALink_KPA1] FOREIGN KEY ([KPALinkedTo]) REFERENCES [Goals].[KPA] ([Id])
);

