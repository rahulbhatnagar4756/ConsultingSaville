CREATE TABLE [audit].[AuditLogDetails] (
    [Id]                   BIGINT         IDENTITY (1, 1) NOT NULL,
    [AuditLogsid]          BIGINT         NOT NULL,
    [AuditLogFieldNamesid] INT            NOT NULL,
    [OldValue]             NVARCHAR (MAX) NULL,
    CONSTRAINT [PK__AuditLog__3214EC07188D558A] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AuditLogDetails_AuditLogFieldNames] FOREIGN KEY ([AuditLogFieldNamesid]) REFERENCES [audit].[AuditLogFieldNames] ([Id]),
    CONSTRAINT [FK_AuditLogDetails_AuditLogs] FOREIGN KEY ([AuditLogsid]) REFERENCES [audit].[AuditLogs] ([Id])
);

