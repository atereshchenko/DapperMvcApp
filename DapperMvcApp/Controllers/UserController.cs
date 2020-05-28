using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DapperMvcApp.Models.Entities;
using DapperMvcApp.Models.Services;
using Microsoft.AspNetCore.Authorization;

namespace DapperMvcApp.Controllers
{
    public class UserController : Controller
    {
        readonly IUserRepository repos;
        public UserController(IUserRepository r)
        {
            repos = r;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await repos.GetUsers());
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            User user = await repos.Get(id);
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
            await repos.Create(user);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            User user = await repos.Get(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        [Authorize]
        [HttpPost]        
        public async Task<IActionResult> Edit(User user)
        {
            await repos.Update(user);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            User user = await repos.Get(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            User user = await repos.Get(id);
            await repos.Delete(user);
            return RedirectToAction("Index");
        }
    }
}
