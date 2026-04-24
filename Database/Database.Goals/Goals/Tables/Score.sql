CREATE TABLE [Goals].[Score](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,           -- Primary Key
    [Companyid] [bigint] NOT NULL,                  -- Company reference
    [Usersid] [bigint] NOT NULL,                    -- User reference
    [DateProcessed] [datetime] NOT NULL             -- Date processed
        CONSTRAINT [DF_Score_DateProcessed] DEFAULT (GETUTCDATE()),
    [ScoringPeriodsid] [int] NOT NULL,              -- Foreign key to ScoringPeriods
    [Score] [decimal](18, 4) NULL,                  -- Score value
    [isEmployee] [bit] NOT NULL                     -- Employee flag: 1 = employee, 0 = manager
        CONSTRAINT [DF_Score_isEmployee] DEFAULT (1),
 CONSTRAINT [PK_Score] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
) WITH (
    PAD_INDEX = OFF, 
    STATISTICS_NORECOMPUTE = OFF, 
    IGNORE_DUP_KEY = OFF, 
    ALLOW_ROW_LOCKS = ON, 
    ALLOW_PAGE_LOCKS = ON, 
    OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
) ON [PRIMARY]
) ON [PRIMARY];
GO

-- Foreign key to ScoringPeriods
ALTER TABLE [Goals].[Score] WITH CHECK 
ADD CONSTRAINT [FK_Score_ScoringPeriods] FOREIGN KEY([ScoringPeriodsid])
REFERENCES [Goals].[ScoringPeriods]([Id]);
GO

-- Ensure foreign key is enforced
ALTER TABLE [Goals].[Score] CHECK CONSTRAINT [FK_Score_ScoringPeriods];
GO

-- Unique index to prevent duplicate scores per Company/User/Period/Stream
CREATE UNIQUE INDEX [UX_Score_Unique]
ON [Goals].[Score](
    [CompanyId],
    [Usersid],
    [ScoringPeriodsid],
    [isEmployee]            -- IMPORTANT: added isEmployee to handle employee/manager streams
);
GO
