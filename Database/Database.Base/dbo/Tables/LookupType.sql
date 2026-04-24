CREATE TABLE [dbo].[LookupType] (
    [Recordid]    INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (255) NOT NULL,
    [Description] VARCHAR (500) NULL,
    [isDeleted]   INT           CONSTRAINT [DF_LookupType_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LookupType] PRIMARY KEY CLUSTERED ([Recordid] ASC)
);

