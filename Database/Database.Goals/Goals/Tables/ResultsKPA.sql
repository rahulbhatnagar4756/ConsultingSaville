CREATE TABLE [Goals].[ResultsKPA](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,             -- Primary Key
    [KPAid] [bigint] NOT NULL,                        -- Foreign Key to Goals.KPA
    [DateCreated] [datetime] NOT NULL                 -- When the record was created
        CONSTRAINT [DF_ResultsKPA_DateCreated] DEFAULT (GETUTCDATE()),
    [RatingPeriodDatesid] [int] NULL,                 -- Foreign Key to Goals.RatingPeriodDates
    [DateAdded] [datetime] NULL,                      -- Optional date added
    [Score] [decimal](18, 4) NOT NULL,               -- Score value
    [isActive] [bit] NOT NULL,                        -- Active flag
    [isDeleted] [bit] NOT NULL,                       -- Deleted flag
    [isEmployee] [bit] NOT NULL                       -- Employee flag
        CONSTRAINT [DF_ResultsKPA_isEmployee] DEFAULT (1),
 CONSTRAINT [PK_ResultsKPA] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
) WITH (
    PAD_INDEX = OFF, 
    STATISTICS_NORECOMPUTE = OFF, 
    IGNORE_DUP_KEY = OFF, 
    ALLOW_ROW_LOCKS = ON, 
    ALLOW_PAGE_LOCKS = ON, 
    OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
) ON [PRIMARY],
 CONSTRAINT [FK_ResultsKPA_KPA] FOREIGN KEY([KPAid])
     REFERENCES [Goals].[KPA]([Id]),
 CONSTRAINT [FK_ResultsKPA_RatingPeriodDates] FOREIGN KEY([RatingPeriodDatesid])
     REFERENCES [Goals].[RatingPeriodDates]([Id])
) ON [PRIMARY];
GO

-- Unique index to prevent duplicate KPA results per period/stream
CREATE UNIQUE INDEX [UX_ResultsKPA_Unique]
ON [Goals].[ResultsKPA](
    [KPAid],
    [RatingPeriodDatesid],
    [isEmployee]
)
WHERE [isDeleted] = 0;  -- only enforce for active records
GO
