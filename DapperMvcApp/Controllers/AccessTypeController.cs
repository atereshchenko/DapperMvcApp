using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DapperMvcApp.Models.Services;
using DapperMvcApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace DapperMvcApp.Controllers
{
    public class AccessTypeController : Controller
    {
        readonly IAccessTypeRepository _accesstype;
        private readonly ILogger<AccessTypeController> _logger;
        public AccessTypeController(ILogger<AccessTypeController> logger, IAccessTypeRepository accesstype)
        {
            _logger = logger;
            _accesstype = accesstype;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _accesstype.ToList());
        }
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            AccessType tmp = await _accesstype.FindById(id);
            if (tmp != null)
                return View(tmp);
            return NotFound();
        }
    }
}
