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
    }
    public class RoleRepository : IRoleRepository
    {
        string connectionString = null;
        public RoleRepository(string conn)
        {
            connectionString = conn;
        }
        public async Task<Role> Get(int id)
        {
            var user = await Task.Run(() => GetRole(id));
            return user;
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await Task.Run(() => GetListRoles());
        }

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
    }
}
