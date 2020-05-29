CREATE TABLE [dbo].[AccessTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Read] [bit] NOT NULL,
	[Write] [bit] NOT NULL,
	[Delete] [bit] NOT NULL,
 CONSTRAINT [PK_AccessTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [dbo].[AccessTypes] ([Title],[Read],[Write],[Delete]) VALUES('Full', 'true','true','true')
INSERT INTO [dbo].[AccessTypes] ([Title],[Read],[Write],[Delete]) VALUES('Change', 'true','true','false')
INSERT INTO [dbo].[AccessTypes] ([Title],[Read],[Write],[Delete]) VALUES('Read', 'true','false','false')
INSERT INTO [dbo].[AccessTypes] ([Title],[Read],[Write],[Delete]) VALUES('Denied', 'false','false','false')

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Email] [nvarchar](100) NULL,
	[Password] [nvarchar](max) NULL,
	[Age] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

INSERT INTO [dbo].[Users] ([Name], [Email], [Password], [Age]) VALUES ('Test User', 'test@mail.ru', 'A1aaaaaa', 10)