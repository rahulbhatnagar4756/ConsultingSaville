CREATE TABLE [Goals].[StatusRanges] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [UUID]           NVARCHAR (200)  CONSTRAINT [DF_StatusRanges_UUID] DEFAULT (newid()) NOT NULL,
    [Statusid]       INT             NOT NULL,
    [StatusRangesid] INT             NULL,
    [Name]           VARCHAR (255)   NOT NULL,
    [Iconsid]        INT             NULL,
    [Color]          NVARCHAR (50)   NOT NULL,
    [Point5Scale]    INT             CONSTRAINT [DF_StatusRanges_Point5Scale] DEFAULT ((1)) NOT NULL,
    [RangeStart]     DECIMAL (10, 2) NULL,
    [Orderval]       INT             CONSTRAINT [DF_StatusRanges_Orderval] DEFAULT ((1)) NOT NULL,
    [isNegative]     INT             NULL,
    [isDeleted]      BIT             CONSTRAINT [DF__StatusRan__isDel__59E54FE7] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_GoalStatusType] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StatusRanges_Status] FOREIGN KEY ([Statusid]) REFERENCES [Goals].[Status] ([Id]),
    CONSTRAINT [FK_StatusRanges_StatusRanges] FOREIGN KEY ([StatusRangesid]) REFERENCES [Goals].[StatusRanges] ([Id])
);

