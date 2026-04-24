CREATE TABLE [Goals].[ContractPeriods] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [UUID]                  NVARCHAR (200) CONSTRAINT [DF_ContractPeriods_UUID] DEFAULT (newid()) NOT NULL,
    [Companyid]             INT            CONSTRAINT [DF_ContractPeriods_Companyid] DEFAULT ((208)) NOT NULL,
    [Name]                  NVARCHAR (200) NOT NULL,
    [Description]           NVARCHAR (MAX) NULL,
    [DateStart]             DATETIME       CONSTRAINT [DF_ContractPeriods_DateStart] DEFAULT (datefromparts(datepart(year,getdate()),(1),(1))) NOT NULL,
    [DateEnd]               DATETIME       CONSTRAINT [DF_ContractPeriods_DateEnd] DEFAULT (dateadd(second,(-1),CONVERT([datetime],datefromparts(datepart(year,getdate())+(1),(1),(1))))) NOT NULL,
    [Year]                  INT            CONSTRAINT [DF_ContractPeriods_Year] DEFAULT (datepart(year,getdate())) NULL,
    [DateTerminationActive] DATETIME       NULL,
    [isActive]              BIT            CONSTRAINT [DF_ContractPeriods_isActive] DEFAULT ((1)) NOT NULL,
    [isDeleted]             BIT            CONSTRAINT [DF_ContractPeriods_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ContractPeriods] PRIMARY KEY CLUSTERED ([Id] ASC)
);

