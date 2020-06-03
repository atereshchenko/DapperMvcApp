using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DapperMvcApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace DapperMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            if (Request.IsHttps)
            {
                ViewData["CurrentHost"] = "https://" + Request.Host;
                ViewData["Privacy"] = "https://" + Request.Host + Request.Path;
            }
            else
            {
                ViewData["CurrentHost"] = "http://" + Request.Host;
                ViewData["Privacy"] = "http://" + Request.Host + Request.Path;
            }            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
