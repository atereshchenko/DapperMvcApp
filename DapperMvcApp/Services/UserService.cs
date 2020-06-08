using Dapper;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DapperMvcApp.Models.Entities;

namespace DapperMvcApp.Models.Services
{
    public interface IUserRepository
    {
        Task<User> FindById(int id);        
        Task<User> FindByEmail(string email);
        Task<User> Get(string email, string password);        
        Task<IEnumerable<User>> ToList();
        Task<IEnumerable<User>> RolesInUser();
        Task<IEnumerable<Role>> RolesInUser(int id);
        Task<User> Create(string email, string password);
        Task<User> Create(User user);
        Task<User> Update(User user);
        Task<User> Delete(User user);        
    }

    public class UserRepository : IUserRepository
    {
        string connectionString = null;
        public UserRepository(string conn)
        {
            connectionString = conn;
        }

        #region public async methods
        public async Task<User> FindById(int id)
        {
            var user = await Task.Run(() => GetUser(id));
            return user;
        }
        public async Task<User> FindByEmail(string email)
        {
            var user = await Task.Run(() => GetUser(email));
            return user;
        }
        public async Task<User> Get(string email, string password)
        {
            var user = await Task.Run(() => GetUser(email, password));
            return user;
        }
        public async Task<IEnumerable<User>> ToList()
        {
            return await Task.Run(() => GetListUsers());
        }
        public async Task<IEnumerable<User>> RolesInUser()
        {
            return await Task.Run(() => RoleInUser());
        }
        public async Task<IEnumerable<Role>> RolesInUser(int id)
        {
            return await Task.Run(() => RoleInUser(id));
        }
        public async Task<User> Create(string email, string password)
        {
            return await Task.Run(() => CreateUser(email, password));
        }
        public async Task<User> Create(User user)
        {
            return await Task.Run(() => CreateUser(user));
        }
        public async Task<User> Update(User user)
        {
            return await Task.Run(() => UpdateUser(user));
        }
        public async Task<User> Delete(User user)
        {
            return await Task.Run(() => DeleteUser(user));
        }
        #endregion

        #region private methods
        private User GetUser(int id)
        {
            string query = "SELECT * FROM Users WHERE Id = @Id;";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>(query, new { Id = id }).FirstOrDefault();
            }
        }
        private User GetUser(string email)
        {
            string query = "SELECT * FROM Users WHERE Email = @email;";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>(query, new { Email = email }).FirstOrDefault();
            }
        }
        private User GetUser(string email, string password)
        {
            string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password;";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>(query, new { Email = email, Password = password }).FirstOrDefault();
            }
        }
        private List<User> GetListUsers()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>("SELECT * FROM Users;").ToList();
            }
        }        
        private User CreateUser(string email, string password)
        {
            User user = new User();
            string query = "INSERT INTO Users (Name, Email, Password) Values (@Name, @Email, @Password); SELECT CAST(SCOPE_IDENTITY() as int);";
            using (IDbConnection db = new SqlConnection(connectionString))
            {                
                int? userId = db.Query<int>(query, new { Name = email, Email = email, Password = password }).FirstOrDefault();
                user = GetUser(userId.Value);
            }

            string query2 = "INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId]) Values (@UserId, @RoleId);";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Query<UserRole>(query2, new { UserId = user.Id, RoleId = 3 }).FirstOrDefault();
            }            
            return user;
        }
        private User CreateUser(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Users (Name, Age, Email) VALUES(@Name, @Age, @Email); SELECT CAST(SCOPE_IDENTITY() as int);";
                int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                user.Id = userId.Value;
                return user;
            }
        }
        private User UpdateUser(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Users SET Name = @Name, Age = @Age, Email = @Email WHERE Id = @Id;";
                db.Execute(sqlQuery, user);                
            }
            return user;
        }
        private User DeleteUser(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Users WHERE Id = @Id;";
                db.Execute(sqlQuery, new { user.Id });
            }
            return user;
        }
        private List<User> RoleInUser()
        {
            string query = "Select [Users].*, [Roles].* " +
                "FROM [dbo].[Users] AS [Users] " +
                "LEFT OUTER JOIN [dbo].[UserRoles] AS UserRoles ON [Users].Id = UserRoles.UserId " +
                "LEFT OUTER JOIN [dbo].[Roles] AS [Roles] on UserRoles.RoleId = [Roles].Id ;";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User, Role, User>(
                        query,
                        (user, role) =>
                        {
                            user.Roles = user.Roles ?? new List<Role>();                        
                            user.Roles.Add(role);
                            return user;
                        },
                        splitOn: "Id"
                    )
                    .GroupBy(o => o.Id)
                    .Select(group =>
                    {
                        var combinedUser = group.First();
                        combinedUser.Roles = group.Select(user => user.Roles.Single()).ToList();
                        return combinedUser;
                    }).ToList();
            }
        }
        private List<Role> RoleInUser(int id)
        {
            string query = "Select [Roles].* " +
                "FROM [dbo].[Users] AS [Users] " +
                "LEFT OUTER JOIN [dbo].[UserRoles] AS UserRoles ON [Users].Id = UserRoles.UserId " +
                "LEFT OUTER JOIN [dbo].[Roles] AS [Roles] on UserRoles.RoleId = [Roles].Id " +
                "Where [Users].Id = @Id;";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Role>(query, new { Id = id }).ToList();
            }
        }
        #endregion
    }
}
