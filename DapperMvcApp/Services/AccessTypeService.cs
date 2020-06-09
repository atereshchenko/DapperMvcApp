using Dapper;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DapperMvcApp.Entities;

namespace DapperMvcApp.Services
{
    public interface IAccessTypeRepository
    {
        /// <summary>
        /// Получить список AccessType
        /// </summary>
        /// <returns>Список AccessType</returns>
        Task<IEnumerable<AccessType>> ToList();

        /// <summary>
        /// Полчить запись AccessType по Id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>AccessType</returns>
        Task<AccessType> FindById(int id);
    }

    public class AccessTypeRepository : IAccessTypeRepository
    {
        string connectionString = null;
        public AccessTypeRepository(string conn)
        {
            connectionString = conn;
        }

        #region public async methods
        public async Task<IEnumerable<AccessType>> ToList()
        {
            return await Task.Run(() => GetItemsAccessTypes());
        }

        public async Task<AccessType> FindById(int id)
        {
            var user = await Task.Run(() => GetItemAccessTypes(id));
            return user;
        }
        #endregion

        #region private methods
        private List<AccessType> GetItemsAccessTypes()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<AccessType>("SELECT * FROM AccessTypes;").ToList();
            }
        }

        private AccessType GetItemAccessTypes(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<AccessType>("SELECT * FROM AccessTypes WHERE Id = @id;", new { id }).FirstOrDefault();
            }
        }
        #endregion
    }
}
