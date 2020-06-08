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
        Task<Role> FindById(int id);
        Task<IEnumerable<Role>> ToList();
        Task<IEnumerable<Role>> UsersInRole();
        Task<IEnumerable<Role>> UsersInRole(int id);
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
        public async Task<Role> FindById(int id)
        {
            var user = await Task.Run(() => GetRole(id));
            return user;
        }
        public async Task<IEnumerable<Role>> ToList()
        {            
            return await Task.Run(() => GetListRoles());
        }
        public async Task<IEnumerable<Role>> UsersInRole()
        {
            return await Task.Run(() => UserInRole());
        }

        public async Task<IEnumerable<Role>> UsersInRole(int id)
        {
            return await Task.Run(() => UserInRole(id));
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
        private List<Role> UserInRole()
        {
            string query = "SELECT [Roles].*, [Users].* " +                
                "FROM [dbo].[Roles] AS [Roles] " +
                "LEFT OUTER JOIN [dbo].[UserRoles] AS UserRoles ON [Roles].Id = UserRoles.RoleId " +
                "LEFT OUTER JOIN [dbo].[Users] AS Users on UserRoles.UserId = Users.Id;";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Role, User, Role>(
                        query,
                        (role, user) =>
                        {
                            role.Users = role.Users ?? new List<User>();
                            role.Users.Add(user);
                            return role;
                        },
                        splitOn: "Id"
                    )
                    .GroupBy(o => o.Id)
                    .Select(group =>
                    {
                        var combinedRole = group.First();
                        combinedRole.Users = group.Select(role => role.Users.Single()).ToList();
                        return combinedRole;
                    }).ToList();               
            }           
        }
        private List<Role> UserInRole(int id)
        {
            string query = "SELECT [Roles].*, [Users].* " +
                "FROM [dbo].[Roles] AS [Roles] " +
                "LEFT OUTER JOIN [dbo].[UserRoles] AS UserRoles ON [Roles].Id = UserRoles.RoleId " +
                "LEFT OUTER JOIN [dbo].[Users] AS Users on UserRoles.UserId = Users.Id " +
                "Where [Roles].[Id] = " + id + ";";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Role, User, Role>(
                        query,
                        (role, user) =>
                        {
                            role.Users = role.Users ?? new List<User>();
                            role.Users.Add(user);
                            return role;
                        },
                        splitOn: "Id"
                    )
                    .GroupBy(o => o.Id)
                    .Select(group =>
                    {
                        var combinedRole = group.First();
                        combinedRole.Users = group.Select(role => role.Users.Single()).ToList();
                        return combinedRole;
                    }).ToList();
            }
        }
        #endregion
    }
}
