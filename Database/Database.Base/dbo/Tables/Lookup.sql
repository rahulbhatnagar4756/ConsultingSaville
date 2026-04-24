CREATE TABLE [dbo].[Lookup] (
    [Recordid]              INT             IDENTITY (1, 1) NOT NULL,
    [Companyid]             INT             NULL,
    [EmployeeDepartmentsid] INT             NULL,
    [Description]           NVARCHAR (2000) NULL,
    [LookupTypeid]          INT             NOT NULL,
    [Lookupid]              INT             NULL,
    [Value]                 NVARCHAR (2000) NOT NULL,
    [OrderVal]              INT             CONSTRAINT [DF_Lookup_OrderVal] DEFAULT ((0)) NOT NULL,
    [isDeleted]             INT             CONSTRAINT [DF_Lookup_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Lookup] PRIMARY KEY CLUSTERED ([Recordid] ASC),
    CONSTRAINT [FK_Lookup_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID]),
    CONSTRAINT [FK_Lookup_Lookup] FOREIGN KEY ([Lookupid]) REFERENCES [dbo].[Lookup] ([Recordid]),
    CONSTRAINT [FK_Lookup_LookupType] FOREIGN KEY ([LookupTypeid]) REFERENCES [dbo].[LookupType] ([Recordid])
);

