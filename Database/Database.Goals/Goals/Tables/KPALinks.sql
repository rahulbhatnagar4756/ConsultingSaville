CREATE TABLE [Goals].[KPALinks] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [UUID]           NVARCHAR (200)  NOT NULL,
    [KPALinkTypesid] INT             NOT NULL,
    [KPAid]          BIGINT          NOT NULL,
    [LinkedId]       BIGINT          NOT NULL,
    [Weight]         DECIMAL (18, 4) CONSTRAINT [DF_KPALink_Weight] DEFAULT ((100)) NOT NULL,
    [isDeleted]      BIT             CONSTRAINT [DF_KPALink_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_KPALink] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_KPALinks_KPA] FOREIGN KEY ([KPAid]) REFERENCES [Goals].[KPA] ([Id]),
    CONSTRAINT [FK_KPALinks_KPALinkTypes] FOREIGN KEY ([KPALinkTypesid]) REFERENCES [Goals].[KPALinkTypes] ([Id])
);

