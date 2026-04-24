CREATE TABLE [Goals].[ToleranceRanges] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [UUID]            NVARCHAR (200)  CONSTRAINT [DF_ToleranceRanges_UUID] DEFAULT (newid()) NOT NULL,
    [ToleranceSetsid] INT             NOT NULL,
    [RangeStart]      DECIMAL (18, 2) NULL,
    [Score]           DECIMAL (18, 2) CONSTRAINT [DF_ToleranceRanges_Score] DEFAULT ((0)) NOT NULL,
    [isDeleted]       BIT             CONSTRAINT [DF_ToleranceRanges_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ToleranceRanges] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ToleranceRanges_ToleranceSets] FOREIGN KEY ([ToleranceSetsid]) REFERENCES [Goals].[ToleranceSets] ([Id])
);

