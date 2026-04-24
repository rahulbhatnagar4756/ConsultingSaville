CREATE TABLE [Goals].[EnterpriseStructureTypes] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UUID]        NVARCHAR (200) CONSTRAINT [DF_EnterpriseStructureTypes_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]   INT            CONSTRAINT [DF_EnterpriseStructureTypes_Companyid] DEFAULT ((208)) NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Iconsid]     INT            CONSTRAINT [DF_EnterpriseStructureTypes_Iconsid] DEFAULT ((18)) NULL,
    [Color]       NVARCHAR (50)  CONSTRAINT [DF_EnterpriseStructureTypes_Color] DEFAULT (N'#362f21') NULL,
    [Orderval]    INT            CONSTRAINT [DF_EnterpriseStructureTypes_Orderval] DEFAULT ((1)) NOT NULL,
    [isDeleted]   BIT            CONSTRAINT [DF_EnterpriseStructureTypes_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EnterpriseStructureTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

