CREATE TABLE [dbo].[EmployeeJobDisciplines] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [UUID]      NVARCHAR (200) CONSTRAINT [DF_EmployeeJobDisciplines_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid] INT            NOT NULL,
    [Name]      NVARCHAR (200) NOT NULL,
    [isDeleted] BIT            CONSTRAINT [DF_EmployeeJobDisciplines_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmployeeJobDisciplines] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeeJobDisciplines_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID])
);

