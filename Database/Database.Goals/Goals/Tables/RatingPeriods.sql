CREATE TABLE [Goals].[RatingPeriods] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UUID]        NVARCHAR (200) CONSTRAINT [DF_RatingPeriods_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]   BIGINT         NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [DisplayName] NVARCHAR (200) NOT NULL,
    [isDeleted]   BIT            CONSTRAINT [DF_RatingPeriods_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RatingPeriods] PRIMARY KEY CLUSTERED ([Id] ASC)
);

