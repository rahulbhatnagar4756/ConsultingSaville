CREATE TABLE [dbo].[EmployeeLevels] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UUID]        NVARCHAR (200) CONSTRAINT [DF_EmployeeLevels_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]   INT            NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [Description] NVARCHAR (500) NULL,
    [LevelOrder]  INT            CONSTRAINT [DF_EmployeeLevels_LevelOrder] DEFAULT ((10)) NOT NULL,
    [isDeleted]   INT            CONSTRAINT [DF_EmployeeLevels_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmployeeLevels] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmployeeLevels_Companies] FOREIGN KEY ([Companyid]) REFERENCES [dbo].[Companies] ([recordID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 is the highest level then 2, 3, 4, ..', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EmployeeLevels', @level2type = N'COLUMN', @level2name = N'LevelOrder';

