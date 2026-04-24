CREATE TABLE [audit].[AuditLogActions] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK__AuditLog__3214EC07E07FBFE6] PRIMARY KEY CLUSTERED ([Id] ASC)
);

