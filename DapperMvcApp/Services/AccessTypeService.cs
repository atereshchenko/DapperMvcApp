using Dapper;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DapperMvcApp.Models.Entities;

namespace DapperMvcApp.Models.Services
{
    public interface IAccessTypeRepository
    {
        /// <summary>
        /// Получить список AccessType
        /// </summary>
        /// <returns>Список AccessType</returns>
        Task<IEnumerable<AccessType>> GetItems();

        /// <summary>
        /// Полчить запись AccessType по Id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>AccessType</returns>
        Task<AccessType> GetItems(int id);
    }

    public class AccessTypeRepository : IAccessTypeRepository
    {
        string connectionString = null;
        public AccessTypeRepository(string conn)
        {
            connectionString = conn;
        }

        public async Task<IEnumerable<AccessType>> GetItems()
        {
            return await Task.Run(() => GetItemsAccessTypes());
        }        

        private List<AccessType> GetItemsAccessTypes()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<AccessType>("SELECT * FROM AccessTypes").ToList();
            }
        }

        public async Task<AccessType> GetItems(int id)
        {
            var user = await Task.Run(() => GetItemAccessTypes(id));
            return user;
        }

        private AccessType GetItemAccessTypes(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<AccessType>("SELECT * FROM AccessTypes WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }
    }
}
