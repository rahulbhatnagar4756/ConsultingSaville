CREATE TABLE [dbo].[Lables] (
    [recordID]    INT            IDENTITY (1, 1) NOT NULL,
    [Lable]       NVARCHAR (MAX) NULL,
    [Description] VARCHAR (500)  NULL,
    [LableTypeID] INT            NULL,
    [LanguageID]  INT            CONSTRAINT [DF_Lables_LanguageID] DEFAULT ((1)) NULL,
    [CompanyID]   INT            CONSTRAINT [DF_Lables_CompanyID] DEFAULT ((1)) NULL,
    [isDeleted]   INT            CONSTRAINT [DF_Lables_isDeleted] DEFAULT ((0)) NULL,
    [isActive]    INT            CONSTRAINT [DF_Lables_isActive] DEFAULT ((1)) NULL,
    [ReferenceID] INT            NULL,
    CONSTRAINT [FK_Lables_LableTypes] FOREIGN KEY ([LableTypeID]) REFERENCES [dbo].[LableTypes] ([recordID]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[Lables] NOCHECK CONSTRAINT [FK_Lables_LableTypes];

