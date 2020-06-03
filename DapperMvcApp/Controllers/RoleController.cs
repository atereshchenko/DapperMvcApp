using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperMvcApp.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperMvcApp.Controllers
{
    public class RoleController : Controller
    {
        readonly IRoleRepository role;
        public RoleController(IRoleRepository _role)
        {
            role = _role;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await role.GetRoles());
        }
    }
}
