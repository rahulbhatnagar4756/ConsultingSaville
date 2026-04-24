CREATE TABLE [Goals].[KPI] (
    [Id]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [UUID]              NVARCHAR (200)  CONSTRAINT [DF_KPI_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]         INT             NOT NULL,
    [Usersid]           INT             NOT NULL,
    [Statusid]          INT             NOT NULL,
    [ToleranceSetsid]   INT             NOT NULL,
    [DateCreated]       DATETIME        CONSTRAINT [DF_KPI_CreateDate] DEFAULT (getutcdate()) NULL,
    [Name]              NVARCHAR (500)  NOT NULL,
    [Description]       NVARCHAR (MAX)  NULL,
    [DateStart]         DATETIME        CONSTRAINT [DF_KPI_DateStart] DEFAULT (getutcdate()) NULL,
    [DateEnd]           DATETIME        NULL,
    [Target]            DECIMAL (18, 4) NOT NULL,
    [isScoreProcessing] BIT             CONSTRAINT [DF_KPI_isScoreProcessing] DEFAULT ((0)) NOT NULL,
    [isActive]          BIT             CONSTRAINT [DF_KPI_isActive] DEFAULT ((1)) NOT NULL,
    [isDeleted]         BIT             CONSTRAINT [DF_KPI_isDeleted] DEFAULT ((0)) NOT NULL,
    [KPIidTemplate]     BIGINT          NULL,
    CONSTRAINT [PK_KPI] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_KPI_Status] FOREIGN KEY ([Statusid]) REFERENCES [Goals].[Status] ([Id]),
    CONSTRAINT [FK_KPI_ToleranceSets] FOREIGN KEY ([ToleranceSetsid]) REFERENCES [Goals].[ToleranceSets] ([Id])
);

