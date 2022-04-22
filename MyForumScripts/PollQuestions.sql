USE [ForumUa]
GO

/****** Object:  Table [dbo].[PollQuestions]    Script Date: 19.04.2022 12:11:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PollQuestions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[PollId] [int] NOT NULL,
	[CountAnswers] [int] NOT NULL,
 CONSTRAINT [PK_PollAnswers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PollQuestions]  WITH CHECK ADD  CONSTRAINT [FK_PollAnswers_Polls] FOREIGN KEY([PollId])
REFERENCES [dbo].[Polls] ([Id])
GO

ALTER TABLE [dbo].[PollQuestions] CHECK CONSTRAINT [FK_PollAnswers_Polls]
GO


