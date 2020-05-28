using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DapperMvcApp.Models.Services;
using DapperMvcApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace DapperMvcApp.Controllers
{
    public class AccessTypeController : Controller
    {
        readonly IAccessTypeRepository repos;
        public AccessTypeController(IAccessTypeRepository r)
        {
            repos = r;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await repos.GetItems());
        }
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            AccessType user = await repos.GetItems(id);
            if (user != null)
                return View(user);
            return NotFound();
        }
    }
}
