--CREATE TABLE [dbo].[AccessTypes]
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

INSERT INTO [dbo].[AccessTypes] ([Title],[Read],[Write],[Delete]) VALUES('Full', 'true','true','true')
INSERT INTO [dbo].[AccessTypes] ([Title],[Read],[Write],[Delete]) VALUES('Change', 'true','true','false')
INSERT INTO [dbo].[AccessTypes] ([Title],[Read],[Write],[Delete]) VALUES('Read', 'true','false','false')
INSERT INTO [dbo].[AccessTypes] ([Title],[Read],[Write],[Delete]) VALUES('Denied', 'false','false','false')

--CREATE TABLE [dbo].[Users]
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

INSERT INTO [dbo].[Users] ([Name], [Email], [Password], [Age]) VALUES ('Admin', 'admin@mail.com', 'A1aaaaaa', 10)
INSERT INTO [dbo].[Users] ([Name], [Email], [Password], [Age]) VALUES ('User1', 'user1@mail.com', 'A1aaaaaa', 10)
INSERT INTO [dbo].[Users] ([Name], [Email], [Password], [Age]) VALUES ('User2', 'user2@mail.com', 'A1aaaaaa', 10)

--CREATE TABLE [dbo].[Roles]
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

INSERT INTO [dbo].[Roles] ([Name]) VALUES ('Administrators')
INSERT INTO [dbo].[Roles] ([Name]) VALUES ('Moderators')
INSERT INTO [dbo].[Roles] ([Name]) VALUES ('Users')

--CREATE TABLE [dbo].[UserRoles]
CREATE TABLE [dbo].[UserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles_RoleId]
GO

ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users_UserId]
GO

INSERT INTO [dbo].[UserRoles]([UserId],[RoleId]) VALUES (1,1)
INSERT INTO [dbo].[UserRoles]([UserId],[RoleId]) VALUES (2,2)
INSERT INTO [dbo].[UserRoles]([UserId],[RoleId]) VALUES (2,3)
INSERT INTO [dbo].[UserRoles]([UserId],[RoleId]) VALUES (3,3)

--Запрос Пользователи в роли (многие-ко-многим)
SELECT [Roles].*, [Users].* 
FROM [dbo].[Roles] AS [Roles] 
INNER JOIN [dbo].[UserRoles] AS UserRoles ON [Roles].Id = UserRoles.RoleId 
INNER JOIN [dbo].[Users] AS Users on UserRoles.UserId = Users.Id;