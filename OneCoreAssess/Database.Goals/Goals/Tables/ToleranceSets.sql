CREATE TABLE [Goals].[ToleranceSets] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UUID]        NVARCHAR (200) CONSTRAINT [DF_ToleranceSets_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]   INT            NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [MaxTotal]    FLOAT (53)     CONSTRAINT [DF_ToleranceSets_MaxTotal] DEFAULT ((100)) NOT NULL,
    [OrderVal]    INT            CONSTRAINT [DF_ToleranceSets_OrderVal] DEFAULT ((1)) NOT NULL,
    [isDeleted]   BIT            CONSTRAINT [DF_ToleranceSets_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ToleranceSets] PRIMARY KEY CLUSTERED ([Id] ASC)
);

