using Dapper;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMvcApp.Models
{
    public interface IUserRepository
    {   
        Task<User> GetUser(int id);        
        Task<IEnumerable<User>> GetUsers();        
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
        public async Task<User> GetUser(int id)
        {
            var user = await Task.Run(() => GetUserId(id));
            return user;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await Task.Run(() => GetListUsers());
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
        
        private User GetUserId(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>("SELECT * FROM Users WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }        
        private List<User> GetListUsers()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>("SELECT * FROM Users").ToList();
            }
        }        
        private User CreateUser(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                //var sqlQuery = "INSERT INTO Users (Name, Age) VALUES(@Name, @Age)";
                //db.Execute(sqlQuery, user);

                // если мы хотим получить id добавленного пользователя
                var sqlQuery = "INSERT INTO Users (Name, Age) VALUES(@Name, @Age); SELECT CAST(SCOPE_IDENTITY() as int)";
                int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                user.Id = userId.Value;
                return user;
            }
        }
        private User UpdateUser(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Users SET Name = @Name, Age = @Age WHERE Id = @Id";
                db.Execute(sqlQuery, user);                
            }
            return user;
        }
        private User DeleteUser(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Users WHERE Id = @Id";
                db.Execute(sqlQuery, new { user.Id });
            }
            return user;
        }
    }
}
