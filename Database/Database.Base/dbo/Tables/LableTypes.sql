CREATE TABLE [dbo].[LableTypes] (
    [recordID]    INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (250) NULL,
    [Description] VARCHAR (250) NULL,
    [Page]        VARCHAR (250) NULL,
    [Type]        VARCHAR (250) NULL,
    [isDeleted]   INT           NULL,
    CONSTRAINT [PK_LableTypes] PRIMARY KEY CLUSTERED ([recordID] ASC)
);

