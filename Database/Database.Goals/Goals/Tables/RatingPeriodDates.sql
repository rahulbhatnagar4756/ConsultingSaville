CREATE TABLE [Goals].[RatingPeriodDates] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [UUID]            NVARCHAR (200) CONSTRAINT [DF_RatingPeriodDates_UUID] DEFAULT (newid()) NOT NULL,
    [RatingPeriodsid] INT            NOT NULL,
    [Name]            NVARCHAR (50)  NOT NULL,
    [DateStart]       DATETIME       NOT NULL,
    [DateEnd]         DATETIME       NULL,
    [DateOpen]        DATETIME       NOT NULL,
    [DateClose]       DATETIME       NULL,
    [isActive]        BIT            CONSTRAINT [DF_RatingPeriodDates_isActive] DEFAULT ((1)) NOT NULL,
    [isDeleted]       BIT            CONSTRAINT [DF_RatingPeriodDates_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RatingPeriodDates] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RatingPeriodDates_RatingPeriods] FOREIGN KEY ([RatingPeriodsid]) REFERENCES [Goals].[RatingPeriods] ([Id])
);

