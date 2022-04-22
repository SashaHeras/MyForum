USE [ForumUa]
GO

/****** Object:  Table [dbo].[UsersPollsAnswers]    Script Date: 19.04.2022 12:12:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UsersPollsAnswers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_UsersPollsAnswers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UsersPollsAnswers]  WITH CHECK ADD  CONSTRAINT [FK_UsersPollsAnswers_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].[UsersPollsAnswers] CHECK CONSTRAINT [FK_UsersPollsAnswers_User]
GO

ALTER TABLE [dbo].[UsersPollsAnswers]  WITH CHECK ADD  CONSTRAINT [FK_UsersPollsAnswers_UsersPollsAnswers] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[PollQuestions] ([Id])
GO

ALTER TABLE [dbo].[UsersPollsAnswers] CHECK CONSTRAINT [FK_UsersPollsAnswers_UsersPollsAnswers]
GO


