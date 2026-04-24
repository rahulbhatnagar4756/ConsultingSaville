CREATE TABLE [Base].[Status] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [UUID]        NVARCHAR (200) CONSTRAINT [DF_Status_UUID] DEFAULT (newid()) NOT NULL,
    [Name]        NVARCHAR (300) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [isDeleted]   BIT            CONSTRAINT [DF_Status_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED ([id] ASC)
);

