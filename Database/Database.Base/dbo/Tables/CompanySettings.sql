CREATE TABLE [dbo].[CompanySettings] (
    [Recordid]              INT            IDENTITY (1, 1) NOT NULL,
    [Companyid]             INT            NOT NULL,
    [CompanySettingTypesid] INT            NOT NULL,
    [Value]                 NVARCHAR (500) NULL,
    [isDeleted]             INT            CONSTRAINT [DF_CompanySettings_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CompanySettings] PRIMARY KEY CLUSTERED ([Recordid] ASC),
    CONSTRAINT [FK_CompanySettings_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID]),
    CONSTRAINT [FK_CompanySettings_CompanySettingtypes] FOREIGN KEY ([CompanySettingTypesid]) REFERENCES [dbo].[CompanySettingtypes] ([Recordid])
);

