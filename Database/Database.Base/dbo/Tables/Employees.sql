CREATE TABLE [dbo].[Employees] (
    [Recordid]    INT      IDENTITY (1, 1) NOT NULL,
    [Userid]      INT      NOT NULL,
    [Companyid]   INT      NOT NULL,
    [DateCreated] DATETIME DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   INT      NOT NULL,
    [DateUpdated] DATETIME DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]   INT      NOT NULL,
    [isdeleted]   INT      DEFAULT ((0)) NOT NULL,
    [isActive]    INT      DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([Recordid] ASC)
);

