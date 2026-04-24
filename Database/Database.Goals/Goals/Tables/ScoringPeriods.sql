CREATE TABLE [Goals].[ScoringPeriods] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [UUID]            NVARCHAR (200)  CONSTRAINT [DF_ScoringPeriods_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]       BIGINT          NOT NULL,
    [Name]            NVARCHAR (200)  NOT NULL,
    [Description]     NVARCHAR (2000) NOT NULL,
    [DateCreated]     DATETIME        CONSTRAINT [DF_ScoringPeriods_DateCreated] DEFAULT (getutcdate()) NOT NULL,
    [StartDay]        INT             NOT NULL,
    [StartMonth]      INT             NOT NULL,
    [EndDay]          INT             NOT NULL,
    [EndMonth]        INT             NOT NULL,
    [DateDeactivated] DATETIME        NULL,
    [isActive]        BIT             CONSTRAINT [DF_ScoringPeriods_isActive] DEFAULT ((1)) NOT NULL,
    [isDeleted]       BIT             CONSTRAINT [DF_ScoringPeriods_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ScoringPeriods] PRIMARY KEY CLUSTERED ([Id] ASC)
);

