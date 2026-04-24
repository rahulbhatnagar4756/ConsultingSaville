CREATE TABLE [audit].[AuditLogFieldNames] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_AuditLogFieldNames] PRIMARY KEY CLUSTERED ([Id] ASC)
);

