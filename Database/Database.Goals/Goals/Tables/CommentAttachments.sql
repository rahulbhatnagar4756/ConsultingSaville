CREATE TABLE [admin].[CommentAttachments](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
     NOT NULL,
    [CommentsId] [bigint] NOT NULL,
     NOT NULL,
     NOT NULL,
     NOT NULL,
    [FileSize] [int] NULL,
     NULL,
    [isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__CommentA__3214EC07D0F5BABD] PRIMARY KEY CLUSTERED 
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
) ON [PRIMARY];
