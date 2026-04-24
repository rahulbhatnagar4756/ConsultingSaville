CREATE TABLE [dbo].[EmployeeHierarchy] (
    [Recordid]       INT            IDENTITY (1, 1) NOT NULL,
    [UUID]           NVARCHAR (200) CONSTRAINT [DF_EmployeeHierarchy_UUID] DEFAULT (newid()) NOT NULL,
    [EmployeeJobsid] INT            NOT NULL,
    [Usersid]        INT            NOT NULL,
    [UsersidManager] INT            NOT NULL,
    [CreateDate]     DATETIME       CONSTRAINT [DF_EmployeeHierarchy_CreateDate] DEFAULT (getdate()) NOT NULL,
    [isDeleted]      INT            CONSTRAINT [DF_EmployeeHierarchy_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmployeeHierarchy] PRIMARY KEY CLUSTERED ([Recordid] ASC),
    CONSTRAINT [FK_EmployeeHierarchy_EmployeeJobs] FOREIGN KEY ([EmployeeJobsid]) REFERENCES [dbo].[EmployeeJobs] ([Recordid]),
    CONSTRAINT [FK_EmployeeHierarchy_users] FOREIGN KEY ([Usersid]) REFERENCES [dbo].[users] ([recordid]),
    CONSTRAINT [FK_EmployeeHierarchy_users1] FOREIGN KEY ([UsersidManager]) REFERENCES [dbo].[users] ([recordid])
);

