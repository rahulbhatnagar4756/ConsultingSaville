CREATE TABLE [dbo].[EmployeeJobsCriticalRoles] (
    [id]                               INT            IDENTITY (1, 1) NOT NULL,
    [UUID]                             NVARCHAR (200) CONSTRAINT [DF_EmployeeJobsCriticalRoles_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]                        INT            NOT NULL,
    [EmployeeJobsCriticalRoleLevelsid] INT            CONSTRAINT [DF_EmployeeJobsCriticalRoles_EmployeeJobsCriticalRoleLevelsid] DEFAULT ((4)) NOT NULL,
    [Name]                             NVARCHAR (255) NOT NULL,
    [Description]                      NVARCHAR (MAX) NULL,
    [isDeleted]                        INT            CONSTRAINT [DF_EmployeeJobsCriticalRoles_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmployeeJobsCriticalRoles] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_EmployeeJobsCriticalRoles_EmployeeJobsCriticalRoleLevels] FOREIGN KEY ([EmployeeJobsCriticalRoleLevelsid]) REFERENCES [dbo].[EmployeeJobsCriticalRoleLevels] ([Id])
);

