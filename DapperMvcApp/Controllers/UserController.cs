using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DapperMvcApp.Models;

namespace DapperMvcApp.Controllers
{
    public class UserController : Controller
    {
        readonly IUserRepository repos;
        public UserController(IUserRepository r)
        {
            repos = r;
        }

        public async Task<IActionResult> Index()
        {
            return View(await repos.GetUsers());
        }

        public async Task<IActionResult> Details(int id)
        {
            User user = await repos.GetUser(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            await repos.Create(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            User user = await repos.GetUser(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            await repos.Update(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            User user = await repos.GetUser(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            User user = await repos.GetUser(id);
            await repos.Delete(user);
            return RedirectToAction("Index");
        }
    }
}
