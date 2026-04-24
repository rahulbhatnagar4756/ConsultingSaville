CREATE TABLE [Goals].[TemplateBusinessUnitDepartmentPositions] (
    [Id]                      INT            IDENTITY (1, 1) NOT NULL,
    [UUID]                    NVARCHAR (200) CONSTRAINT [DF_TemplateBusinessUnitDepartmentPositions_UUID] DEFAULT (newid()) NOT NULL,
    [Templatesid]             INT            NOT NULL,
    [EmployeeBusinessUnitsid] INT            NOT NULL,
    [EmployeeDepartmentsid]   INT            NULL,
    [EmployeeJobsid]          INT            NULL,
    [isDeleted]               BIT            CONSTRAINT [DF_TemplateBusinessUnitDepartmentPositions_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TemplateBusinessUnitDepartmentPositions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TemplateBusinessUnitDepartmentPositions_Templates] FOREIGN KEY ([Templatesid]) REFERENCES [Goals].[Templates] ([Id])
);

