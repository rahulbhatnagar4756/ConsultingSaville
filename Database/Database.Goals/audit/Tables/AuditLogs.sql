CREATE TABLE [audit].[AuditLogs] (
    [Id]                    BIGINT        IDENTITY (1, 1) NOT NULL,
    [CompanyId]             INT           NOT NULL,
    [UsersID]               INT           NOT NULL,
    [AuditLogsTableNamesid] INT           NOT NULL,
    [RecordID]              INT           NOT NULL,
    [AuditLogActionsid]     INT           NOT NULL,
    [AuditDate]             DATETIME2 (7) CONSTRAINT [DF__AuditLogs__Audit__4FE94666] DEFAULT (getutcdate()) NOT NULL,
    [UserIPAddress]         NVARCHAR (50) NULL,
    CONSTRAINT [PK__AuditLog__3214EC0732629BBA] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AuditLogs_AuditLogActions] FOREIGN KEY ([AuditLogActionsid]) REFERENCES [audit].[AuditLogActions] ([Id]),
    CONSTRAINT [FK_AuditLogs_AuditLogTableNames] FOREIGN KEY ([AuditLogsTableNamesid]) REFERENCES [audit].[AuditLogTableNames] ([Id])
);

