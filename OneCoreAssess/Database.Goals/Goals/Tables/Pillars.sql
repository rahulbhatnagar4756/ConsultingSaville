CREATE TABLE [Goals].[Pillars] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UUID]        NVARCHAR (200) CONSTRAINT [DF_Pillars_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]   INT            NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [IconsId]     INT            CONSTRAINT [DF_Pillars_IconsId] DEFAULT ((7)) NULL,
    [IconColor]   NVARCHAR (30)  CONSTRAINT [DF_Pillars_IconColor] DEFAULT (N'#362f21') NULL,
    [OrderVal]    INT            CONSTRAINT [DF_Pillars_OrderVal] DEFAULT ((1)) NOT NULL,
    [isDeleted]   BIT            CONSTRAINT [DF_Pillars_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Pillars] PRIMARY KEY CLUSTERED ([Id] ASC)
);

