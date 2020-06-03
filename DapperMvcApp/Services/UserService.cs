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
        /// <summary>
        /// Выбрать пользователя по ИД
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>модель User</returns>
        Task<User> Get(int id);        

        /// <summary>
        /// Выбрать пользователя по Email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>модель User</returns>
        Task<User> Get(string email);        

        /// <summary>
        /// Выбрать пользователя по Email и Password
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <param name="password">Password пользователя</param>
        /// <returns>модель User</returns>
        Task<User> Get(string email, string password);

        /// <summary>
        /// Выбрать список пользователей
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        Task<IEnumerable<User>> GetUsers();

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="email">Емайл</param>
        /// <param name="password">Пароль</param>
        /// <returns>Пользователь</returns>
        Task<User> Create(string email, string password);

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="user">Модель Пользователя</param>
        /// <returns>Пользователь</returns>
        Task<User> Create(User user);

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <param name="user">Модель Пользователя</param>
        /// <returns>Пользователь</returns>
        Task<User> Update(User user);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="user">Модель Пользователя</param>
        /// <returns>Пользователь</returns>
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
        public async Task<User> Get(int id)
        {
            var user = await Task.Run(() => GetUser(id));
            return user;
        }
        public async Task<User> Get(string email)
        {
            var user = await Task.Run(() => GetUser(email));
            return user;
        }
        public async Task<User> Get(string email, string password)
        {
            var user = await Task.Run(() => GetUser(email, password));
            return user;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await Task.Run(() => GetListUsers());
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
            string query = "INSERT INTO Users (Name, Email, Password) Values (@Name, @Email, @Password);";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>(query, new {Name = email, Email = email, Password = password }).FirstOrDefault();
            }
        }
        private User CreateUser(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                //var sqlQuery = "INSERT INTO Users (Name, Age) VALUES(@Name, @Age)";
                //db.Execute(sqlQuery, user);

                // если мы хотим получить id добавленного пользователя
                var sqlQuery = "INSERT INTO Users (Name, Age) VALUES(@Name, @Age); SELECT CAST(SCOPE_IDENTITY() as int);";
                int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                user.Id = userId.Value;
                return user;
            }
        }
        private User UpdateUser(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Users SET Name = @Name, Age = @Age WHERE Id = @Id;";
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
        #endregion
    }
}
