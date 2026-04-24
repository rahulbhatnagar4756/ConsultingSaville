CREATE TABLE [dbo].[EmployeeJobsDescription] (
    [Recordid]              INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeJobsid]        INT            NOT NULL,
    [EmployeeDepartmentsid] INT            NULL,
    [Companyid]             INT            NULL,
    [Description]           NVARCHAR (MAX) CONSTRAINT [DF_EmployeeJobsDescription_Description] DEFAULT (N'Profile to be Added') NULL,
    [Education]             NVARCHAR (MAX) CONSTRAINT [DF_EmployeeJobsDescription_Education] DEFAULT (N'Profile to be Added') NULL,
    [Experience]            NVARCHAR (MAX) CONSTRAINT [DF_EmployeeJobsDescription_Experience] DEFAULT (N'Profile to be Added') NULL,
    [Competencies]          NVARCHAR (MAX) CONSTRAINT [DF_EmployeeJobsDescription_Competencies] DEFAULT (N'Profile to be Added') NULL,
    [ExpectedSkills]        NVARCHAR (MAX) NULL,
    [WorkingBoundaries]     NVARCHAR (MAX) NULL,
    [BrowserTitle]          NVARCHAR (60)  NULL,
    [isDeleted]             INT            CONSTRAINT [DF_EmployeeJobsDescription_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmployeeJobsDescription] PRIMARY KEY CLUSTERED ([Recordid] ASC),
    CONSTRAINT [FK_EmployeeJobsDescription_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID]) ON DELETE CASCADE,
    CONSTRAINT [FK_EmployeeJobsDescription_EmployeeDepartments] FOREIGN KEY ([EmployeeDepartmentsid]) REFERENCES [dbo].[EmployeeDepartments] ([Recordid]) ON DELETE CASCADE,
    CONSTRAINT [FK_EmployeeJobsDescription_EmployeeJobs] FOREIGN KEY ([EmployeeJobsid]) REFERENCES [dbo].[EmployeeJobs] ([Recordid]) ON DELETE CASCADE
);

