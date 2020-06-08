https://dapper-tutorial.net/ru/knowledge-base/29610051/

SELECT * FROM [DapperMvcApp].[dbo].[Users]
SELECT * FROM [DapperMvcApp].[dbo].[Roles]
SELECT * FROM [DapperMvcApp].[dbo].[UserRoles]

SELECT Users.*, [Roles].*
FROM [dbo].[Users] AS Users
INNER JOIN [dbo].[UserRoles] AS UserRoles ON Users.Id = UserRoles.UserId
INNER JOIN [dbo].[Roles] AS [Roles] ON UserRoles.RoleId = [Roles].Id