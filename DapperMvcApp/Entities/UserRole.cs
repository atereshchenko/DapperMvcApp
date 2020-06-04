using Dapper.Contrib.Extensions;
using DapperMvcApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperMvcApp.Models.Entities
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Dapper.Contrib.Extensions.Key]
        public int UserId { get; set; }

        [Dapper.Contrib.Extensions.Key]
        public int RoleId { get; set; }

        public virtual List<Role> Roles { get; set; }

        public virtual List<User> Users { get; set; }

        public UserRole()
        {
            Roles = new List<Role>();
            Users = new List<User>();
        }
    }
}
