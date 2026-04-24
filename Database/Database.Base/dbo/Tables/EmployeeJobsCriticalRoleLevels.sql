CREATE TABLE [dbo].[EmployeeJobsCriticalRoleLevels] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [OrderVal]    INT            CONSTRAINT [DF_EmployeeJobsCriticalRoleLevels_OrderVal] DEFAULT ((1)) NOT NULL,
    [isDeleted]   INT            CONSTRAINT [DF_EmployeeJobsCriticalRoleLevels_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmployeeJobsCriticalRoleLevels] PRIMARY KEY CLUSTERED ([Id] ASC)
);

