CREATE TABLE [dbo].[EmployeeJobs] (
    [Recordid]                    INT            IDENTITY (1, 1) NOT NULL,
    [UUID]                        NVARCHAR (200) CONSTRAINT [DF_EmployeeJobs_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]                   INT            NULL,
    [EmployeeJobDisciplinesid]    INT            NULL,
    [EmployeeJobsCriticalRolesid] INT            NULL,
    [EmployeeLevelsid]            INT            NULL,
    [Jobsid]                      INT            NULL,
    [Name]                        NVARCHAR (255) NOT NULL,
    [Code]                        NVARCHAR (50)  NULL,
    [isDeleted]                   INT            CONSTRAINT [DF_EmployeeJobs_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmployeeJobs] PRIMARY KEY CLUSTERED ([Recordid] ASC),
    CONSTRAINT [FK_EmployeeJobs_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID]),
    CONSTRAINT [FK_EmployeeJobs_EmployeeJobDisciplines] FOREIGN KEY ([EmployeeJobDisciplinesid]) REFERENCES [dbo].[EmployeeJobDisciplines] ([Id]),
    CONSTRAINT [FK_EmployeeJobs_EmployeeJobsCriticalRoles] FOREIGN KEY ([EmployeeJobsCriticalRolesid]) REFERENCES [dbo].[EmployeeJobsCriticalRoles] ([id]),
    CONSTRAINT [FK_EmployeeJobs_EmployeeLevels] FOREIGN KEY ([EmployeeLevelsid]) REFERENCES [dbo].[EmployeeLevels] ([Id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'newid()', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EmployeeJobs';

