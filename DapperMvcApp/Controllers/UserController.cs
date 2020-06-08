using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DapperMvcApp.Models.Entities;
using DapperMvcApp.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace DapperMvcApp.Controllers
{
    public class UserController : Controller
    {
        readonly IUserRepository _user;
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger, IUserRepository user)
        {
            _logger = logger;
            _user = user;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _user.ToList());
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            User user = await _user.FindById(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]        
        public async Task<IActionResult> Create(User user)
        {
            await _user.Create(user);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            User user = await _user.FindById(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        [Authorize]
        [HttpPost]        
        public async Task<IActionResult> Edit(User user)
        {
            await _user.Update(user);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Roles()
        {
            return View(await _user.RolesInUser());
        }

        [Authorize]
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            User user = await _user.FindById(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            User user = await _user.FindById(id);
            await _user.Delete(user);
            return RedirectToAction("Index");
        }
    }
}
