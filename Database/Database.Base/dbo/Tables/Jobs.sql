CREATE TABLE [dbo].[Jobs] (
    [recordID]    INT            IDENTITY (1, 1) NOT NULL,
    [UUID]        NVARCHAR (200) CONSTRAINT [DF_Jobs_UUID] DEFAULT (newid()) NOT NULL,
    [JobName]     VARCHAR (250)  NULL,
    [JobType]     VARCHAR (100)  NULL,
    [DateCreated] DATETIME       NULL,
    [isdeleted]   INT            NULL,
    [CompanyID]   INT            NULL,
    [DateUpdated] DATETIME       NULL,
    [isCritical]  INT            NULL,
    CONSTRAINT [PK__Jobs__D825197E2705E3B8] PRIMARY KEY CLUSTERED ([recordID] ASC),
    CONSTRAINT [FK_Jobs_Companies] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Companies] ([recordID]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[Jobs] NOCHECK CONSTRAINT [FK_Jobs_Companies];

