using Dapper.Contrib.Extensions;
using DapperMvcApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMvcApp.Entities
{
    [Table("UserRoles")]
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class ChangeRoleViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public List<Role> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        public ChangeRoleViewModel()
        {
            AllRoles = new List<Role>();
            UserRoles = new List<string>();
        }
    }
}
