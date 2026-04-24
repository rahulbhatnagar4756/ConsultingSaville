CREATE TABLE [Goals].[LogDataChanges] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompanyId]       INT            NOT NULL,
    [UsersIdLoggedIn] INT            NOT NULL,
    [TableNameId]     INT            NOT NULL,
    [TableId]         INT            NOT NULL,
    [DateLogged]      DATETIME       CONSTRAINT [DF__LogDataCh__DateL__5C02A283] DEFAULT (getutcdate()) NULL,
    [TableName]       NVARCHAR (100) NOT NULL,
    [ChangeType]      NVARCHAR (10)  NOT NULL,
    [ColumnName]      NVARCHAR (100) NULL,
    [OldValue]        NVARCHAR (MAX) NULL,
    [NewValue]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK__LogDataC__3214EC079A2D9771] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK__LogDataCh__Chang__5CF6C6BC] CHECK ([ChangeType]='DELETE' OR [ChangeType]='UPDATE' OR [ChangeType]='INSERT')
);

