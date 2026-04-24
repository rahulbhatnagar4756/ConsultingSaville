CREATE TABLE [dbo].[AssessmentUsers] (
    [Id]      BIGINT IDENTITY (1, 1) NOT NULL,
    [Usersid] BIGINT NOT NULL,
    [Testsid] INT    NOT NULL,
    CONSTRAINT [PK_AssessmentUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

