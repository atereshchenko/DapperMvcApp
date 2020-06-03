using Dapper;
using DapperMvcApp.Models.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMvcApp.Models.Services
{
    public interface IRoleRepository
    {
        Task<Role> Get(int id);
        Task<IEnumerable<Role>> GetRoles();
        Task<Role> Create(Role _role);
        Task<Role> Update(Role _role);
    }
    public class RoleRepository : IRoleRepository
    {
        string connectionString = null;
        public RoleRepository(string conn)
        {
            connectionString = conn;
        }

        #region public async methods
        public async Task<Role> Get(int id)
        {
            var user = await Task.Run(() => GetRole(id));
            return user;
        }
        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await Task.Run(() => GetListRoles());
        }        
        public async Task<Role> Create(Role _role)
        {
            return await Task.Run(() => CreateRole(_role));
        }
        public async Task<Role> Update(Role _role)
        {
            return await Task.Run(() => UpdateRole(_role));
        }
        #endregion

        #region private methods
        private Role GetRole(int id)
        {
            string query = "SELECT * FROM Roles WHERE Id = @Id;";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Role>(query, new { Id = id }).FirstOrDefault();
            }
        }
        private List<Role> GetListRoles()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Role>("SELECT * FROM Roles;").ToList();
            }
        }
        private Role CreateRole(Role _role)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                //var sqlQuery = "INSERT INTO Users (Name, Age) VALUES(@Name, @Age)";
                //db.Execute(sqlQuery, user);

                // если мы хотим получить id добавленного пользователя
                var sqlQuery = "INSERT INTO Roles (Name) VALUES(@Name); SELECT CAST(SCOPE_IDENTITY() as int);";
                int? roleId = db.Query<int>(sqlQuery, _role).FirstOrDefault();
                _role.Id = roleId.Value;
                return _role;
            }
        }
        private Role UpdateRole(Role _role)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Roles SET Name = @Name WHERE Id = @Id;";
                db.Execute(sqlQuery, _role);
            }
            return _role;
        }
        #endregion
    }
}
