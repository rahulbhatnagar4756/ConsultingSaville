CREATE TABLE [Goals].[Results] (
    [Id]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [KPIid]               BIGINT          NOT NULL,
    [DateCreated]         DATETIME        CONSTRAINT [DF_Results_DateCreated] DEFAULT (getutcdate()) NOT NULL,
    [RatingPeriodDatesid] INT             NULL,
    [DateAdded]           DATETIME        NULL,
    [Result]              DECIMAL (18, 4) NOT NULL,
    [Score]              DECIMAL (18, 4) NOT NULL,
    [isActive]            BIT             CONSTRAINT [DF_Results_isActive] DEFAULT ((1)) NOT NULL,
    [isDeleted]           BIT             CONSTRAINT [DF_Results_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Results] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Results_KPI] FOREIGN KEY ([KPIid]) REFERENCES [Goals].[KPI] ([Id]),
    CONSTRAINT [FK_Results_RatingPeriodDates] FOREIGN KEY ([RatingPeriodDatesid]) REFERENCES [Goals].[RatingPeriodDates] ([Id])
);

