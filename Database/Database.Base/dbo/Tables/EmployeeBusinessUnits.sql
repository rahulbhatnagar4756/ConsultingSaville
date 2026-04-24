CREATE TABLE [dbo].[EmployeeBusinessUnits] (
    [Recordid]                    INT            IDENTITY (1, 1) NOT NULL,
    [UUID]                        NVARCHAR (200) CONSTRAINT [DF_EmployeeBusinessUnits_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]                   INT            NOT NULL,
    [EmployeeBusinessUnitTypesid] INT            NULL,
    [Name]                        NVARCHAR (255) NOT NULL,
    [Description]                 NVARCHAR (500) NULL,
    [IconId]                      INT            NULL,
    [IconColor]                   NVARCHAR (30)  NULL,
    [isDeleted]                   INT            CONSTRAINT [DF_EmployeeBusinessUnits_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmployeeBusinessUnits] PRIMARY KEY CLUSTERED ([Recordid] ASC),
    CONSTRAINT [FK_EmployeeBusinessUnits_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID]),
    CONSTRAINT [FK_EmployeeBusinessUnits_EmployeeBusinessUnitTypes] FOREIGN KEY ([EmployeeBusinessUnitTypesid]) REFERENCES [dbo].[EmployeeBusinessUnitTypes] ([Id])
);

