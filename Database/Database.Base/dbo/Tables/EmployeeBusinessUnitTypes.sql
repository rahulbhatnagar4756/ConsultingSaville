CREATE TABLE [dbo].[EmployeeBusinessUnitTypes] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [UUID]      NVARCHAR (200) CONSTRAINT [DF_EmployeeBusinessUnitTypes_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid] INT            NOT NULL,
    [Name]      NVARCHAR (255) NOT NULL,
    [Iconsid]   INT            NULL,
    [IconColor] NVARCHAR (30)  CONSTRAINT [DF_EmployeeBusinessUnitTypes_IconColor] DEFAULT (N'#ffffff') NULL,
    [isDeleted] BIT            CONSTRAINT [DF_EmployeeBusinessUnitTypes_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmployeeBusinessUnitTypes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeeBusinessUnitTypes_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID])
);

