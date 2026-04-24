CREATE TABLE [Goals].[Status] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UUID]        NVARCHAR (200) CONSTRAINT [DF_Status_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]   INT            NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (500) NULL,
    [Orderval]    INT            CONSTRAINT [DF_Status_Orderval] DEFAULT ((1)) NOT NULL,
    [isDefault]   BIT            CONSTRAINT [DF_Status_isDefault] DEFAULT ((0)) NOT NULL,
    [isDeleted]   BIT            CONSTRAINT [DF_Status_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_GoalStatusSets] PRIMARY KEY CLUSTERED ([Id] ASC)
);

