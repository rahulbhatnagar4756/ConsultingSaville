CREATE TABLE [dbo].[UserDataStore] (
    [Recordid]         INT            IDENTITY (1, 1) NOT NULL,
    [UUID]             NVARCHAR (200) CONSTRAINT [DF_UserDataStore_UUID] DEFAULT (newid()) NOT NULL,
    [Userid]           INT            NOT NULL,
    [UserDataFieldsID] INT            NOT NULL,
    [CreatedDate]      DATETIME       CONSTRAINT [DF_UserDataStore_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [UsersidCreatedBy] INT            NOT NULL,
    [intValue]         INT            CONSTRAINT [DF_UserDataStore_intValue] DEFAULT ((0)) NOT NULL,
    [txtValue]         NVARCHAR (MAX) NOT NULL,
    [dblValue]         FLOAT (53)     CONSTRAINT [DF_UserDataStore_dblValue] DEFAULT ((0)) NOT NULL,
    [dateValue]        DATETIME       CONSTRAINT [DF_UserDataStore_dateValue] DEFAULT ((0)) NOT NULL,
    [Orderval]         INT            CONSTRAINT [DF_UserDataStore_Orderval] DEFAULT ((1)) NOT NULL,
    [isDeleted]        INT            CONSTRAINT [DF_UserDataStore_isDeleted] DEFAULT ((0)) NOT NULL,
    [CompanyID]        INT            NOT NULL,
    [ProjectID]        INT            CONSTRAINT [DF_UserDataStore_ProjectID] DEFAULT ((0)) NOT NULL,
    [JobID]            INT            CONSTRAINT [DF_UserDataStore_JobID] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UserDataStore] PRIMARY KEY CLUSTERED ([Recordid] ASC),
    CONSTRAINT [FK_UserDataStore_Companies] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Companies] ([recordID]),
    CONSTRAINT [FK_UserDataStore_users] FOREIGN KEY ([Userid]) REFERENCES [dbo].[users] ([recordid])
);

