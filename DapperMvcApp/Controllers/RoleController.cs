using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperMvcApp.Models.Entities;
using DapperMvcApp.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperMvcApp.Controllers
{
    public class RoleController : Controller
    {
        readonly IRoleRepository _role;
        readonly IUserRepository _user;

        public RoleController(IRoleRepository role, IUserRepository userManager)
        {
            _role = role;
            _user = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _role.ToList());
        }

        [Authorize]
        public async Task<IActionResult> UserList()
        {
            return View(await _user.ToList());
        }
        
        [Authorize]
        public IActionResult Create()
        {            
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Role role)
        {
            await _role.Create(role);
            return RedirectToAction("Index");
        }
        
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            Role role = await _role.FindById(id);
            if (role != null)
                return View(role);
            return NotFound();
        }        

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Role role)
        {
            await _role.Update(role);
            return RedirectToAction("Index");
        }
    }
}
