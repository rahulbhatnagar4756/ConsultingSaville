CREATE TABLE [Goals].[KPA] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [UUID]            NVARCHAR (200) CONSTRAINT [DF_KPA_UUID] DEFAULT (newid()) NOT NULL,
    [CompanyId]       INT            NOT NULL,
    [Usersid]         INT            NOT NULL,
    [Statusid]        INT            NOT NULL,
    [RatingPeriodsid] INT            NULL,
    [DateCreated]     DATETIME       CONSTRAINT [DF_KPA_DateCreated] DEFAULT (getutcdate()) NOT NULL,
    [DateEnded]       DATETIME       NULL,
    [Name]            NVARCHAR (500) NOT NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [DateLastActive]  DATETIME       NULL,
    [isActive]        BIT            CONSTRAINT [DF_KPA_isActive] DEFAULT ((1)) NOT NULL,
    [isDeleted]       BIT            CONSTRAINT [DF_KPA_isDeleted] DEFAULT ((0)) NOT NULL,
    [KPAidTemplate]   BIGINT         NULL,
    CONSTRAINT [PK_KPA] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_KPA_RatingPeriods] FOREIGN KEY ([RatingPeriodsid]) REFERENCES [Goals].[RatingPeriods] ([Id])
);

