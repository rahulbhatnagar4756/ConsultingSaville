CREATE TABLE [dbo].[CompanySettingtypes] (
    [Recordid]  INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [isDeleted] INT           CONSTRAINT [DF_CompanySettingtypes_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CompanySettingtypes] PRIMARY KEY CLUSTERED ([Recordid] ASC)
);

