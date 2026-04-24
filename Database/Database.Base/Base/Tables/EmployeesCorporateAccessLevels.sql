CREATE TABLE [Base].[EmployeesCorporateAccessLevels] (
    [Id]                          INT            IDENTITY (1, 1) NOT NULL,
    [UUID]                        NVARCHAR (200) CONSTRAINT [DF_EmployeesCorporateAccessLevels_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]                   INT            NOT NULL,
    [Usersid]                     INT            NOT NULL,
    [EmployeeBusinessUnitTypesid] INT            NULL,
    [EmployeeBusinessUnitsid]     INT            NULL,
    [EmployeeDepartmentsid]       INT            NULL,
    [isFullAccess]                BIT            CONSTRAINT [DF_EmployeesCorporateAccessLevels_isFullAccess] DEFAULT ((0)) NOT NULL,
    [isActive]                    BIT            CONSTRAINT [DF_EmployeesCorporateAccessLevels_isActive] DEFAULT ((1)) NOT NULL,
    [isDeleted]                   BIT            CONSTRAINT [DF_EmployeesCorporateAccessLevels_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmployeesCorporateAccessLevels] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeesCorporateAccessLevels_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID]),
    CONSTRAINT [FK_EmployeesCorporateAccessLevels_EmployeeBusinessUnits] FOREIGN KEY ([EmployeeBusinessUnitsid]) REFERENCES [dbo].[EmployeeBusinessUnits] ([Recordid]),
    CONSTRAINT [FK_EmployeesCorporateAccessLevels_EmployeeBusinessUnitTypes] FOREIGN KEY ([EmployeeBusinessUnitTypesid]) REFERENCES [dbo].[EmployeeBusinessUnitTypes] ([Id]),
    CONSTRAINT [FK_EmployeesCorporateAccessLevels_EmployeeDepartments] FOREIGN KEY ([EmployeeDepartmentsid]) REFERENCES [dbo].[EmployeeDepartments] ([Recordid]),
    CONSTRAINT [FK_EmployeesCorporateAccessLevels_users] FOREIGN KEY ([Usersid]) REFERENCES [dbo].[users] ([recordid])
);

