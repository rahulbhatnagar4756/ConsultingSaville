CREATE TABLE [dbo].[EmployeeDepartments] (
    [Recordid]                INT            IDENTITY (1, 1) NOT NULL,
    [UUID]                    NVARCHAR (200) CONSTRAINT [DF_EmployeeDepartments_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]               INT            NOT NULL,
    [EmployeeBusinessUnitsid] INT            NULL,
    [Name]                    NVARCHAR (255) NOT NULL,
    [Description]             NVARCHAR (MAX) NULL,
    [IconId]                  INT            NULL,
    [IconColor]               NVARCHAR (30)  NULL,
    [isDeleted]               INT            CONSTRAINT [DF_EmployeeDepartments_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmployeeDepartments] PRIMARY KEY CLUSTERED ([Recordid] ASC),
    CONSTRAINT [FK_EmployeeDepartments_EmployeeBusinessUnits] FOREIGN KEY ([EmployeeBusinessUnitsid]) REFERENCES [dbo].[EmployeeBusinessUnits] ([Recordid])
);

