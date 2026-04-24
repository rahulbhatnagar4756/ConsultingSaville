CREATE TABLE [dbo].[TableNames] (
    [Recordid]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (255)  NOT NULL,
    [NameSpace] NVARCHAR (500) NULL,
    CONSTRAINT [PK_TableNames] PRIMARY KEY CLUSTERED ([Recordid] ASC)
);

