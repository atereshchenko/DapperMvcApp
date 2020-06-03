using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperMvcApp.Models.Entities;
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

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Role _role)
        {
            await role.Create(_role);
            return RedirectToAction("Index");
        }
        
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            Role _role = await role.Get(id);
            if (_role != null)
                return View(_role);
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Role _role)
        {
            await role.Update(_role);
            return RedirectToAction("Index");
        }

    }
}
