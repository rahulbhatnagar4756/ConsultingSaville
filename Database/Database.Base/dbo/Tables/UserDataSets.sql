CREATE TABLE [dbo].[UserDataSets] (
    [Recordid]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (255) NOT NULL,
    [UserDataSetTypes] NVARCHAR (100) NOT NULL,
    [isDeleted]        INT            CONSTRAINT [DF_UserDataSets_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UserDataSets] PRIMARY KEY CLUSTERED ([Recordid] ASC)
);

