CREATE TABLE [admin].[Comments](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
     NOT NULL,
    [Companyid] [bigint] NOT NULL,
    [Usersid] [bigint] NOT NULL,
    [TableNameid] [int] NOT NULL,
    [TableTargetid] [bigint] NOT NULL,
    [CreateDate] [datetime] NOT NULL,
    [Comment] [nvarchar](max) NOT NULL,
    [CommentTypesid] [int] NULL,
    [isDeleted] [bit] NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (
    PAD_INDEX = OFF, 
    STATISTICS_NORECOMPUTE = OFF, 
    IGNORE_DUP_KEY = OFF, 
    ALLOW_ROW_LOCKS = ON, 
    ALLOW_PAGE_LOCKS = ON, 
    OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
