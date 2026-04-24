CREATE TABLE [dbo].[ControlInputTypes] (
    [Recordid]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    [isDeleted] INT            CONSTRAINT [DF_ControlInputType_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ControlInputTypeType] PRIMARY KEY CLUSTERED ([Recordid] ASC)
);

