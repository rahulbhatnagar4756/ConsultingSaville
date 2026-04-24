USE [Goals]
GO

/** Object:  Table [Goals].[ResultsPillars]    Script Date: 2025/12/31 08:13:32 **/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Goals].[ResultsPillars](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [Companyid] [int] NOT NULL,
    [Usersid] [int] NOT NULL,
    [RatingPeriodDatesid] [int] NOT NULL,
    [Pillarsid] [int] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    [DateAdded] [datetime] NOT NULL,
    [Score] [decimal](18, 4) NULL,
    [isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ResultsPillars] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Goals].[ResultsPillars] ADD  CONSTRAINT [DF_ResultsPillars_DateAdded]  DEFAULT (getutcdate()) FOR [DateAdded]
GO

ALTER TABLE [Goals].[ResultsPillars] ADD  CONSTRAINT [DF_ResultsPillars_isDeleted]  DEFAULT ((0)) FOR [isDeleted]
GO

ALTER TABLE [Goals].[ResultsPillars]  WITH CHECK ADD  CONSTRAINT [FK_ResultsPillars_Pillars] FOREIGN KEY([Pillarsid])
REFERENCES [Goals].[Pillars] ([Id])
GO

ALTER TABLE [Goals].[ResultsPillars] CHECK CONSTRAINT [FK_ResultsPillars_Pillars]
GO

ALTER TABLE [Goals].[ResultsPillars]  WITH CHECK ADD  CONSTRAINT [FK_ResultsPillars_RatingPeriodDates] FOREIGN KEY([RatingPeriodDatesid])
REFERENCES [Goals].[RatingPeriodDates] ([Id])
GO

ALTER TABLE [Goals].[ResultsPillars] CHECK CONSTRAINT [FK_ResultsPillars_RatingPeriodDates]
GO

CREATE UNIQUE INDEX UX_ResultsPillars_Unique
ON [Goals].[ResultsPillars]
(
    Companyid,
    Usersid,
    Pillarsid,
    RatingPeriodDatesid
)
WHERE isDeleted = 0;
