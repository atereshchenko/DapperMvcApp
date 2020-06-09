using System.Diagnostics;
using System.Threading.Tasks;
using DapperMvcApp.Models.Entities;
using DapperMvcApp.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DapperMvcApp.Models;



namespace DapperMvcApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleRepository _role;
        private readonly IUserRepository _user;

        public RoleController(ILogger<RoleController> logger, IRoleRepository role, IUserRepository userManager)
        {
            _logger = logger;
            _role = role;
            _user = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _role.ToList());
        }

        [Authorize]
        public async Task<IActionResult> Users()
        {            
            return View(await _role.UsersInRole());
        }

        [Authorize]
        public async Task<IActionResult> UsersInRole(int id)
        {
            return View(await _role.UsersInRole(id));
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

        [Authorize]        
        public async Task<IActionResult> Change(string userId)
        {
            int _id = int.Parse(userId);
            User user = await _user.FindById(_id);
            if (user != null)
            {
                var userRoles = await _user.RolesInUser(user.Id);                
                var allRoles = await _role.ToListRole();
                ChangeRoleModel model = new ChangeRoleModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles, 
                    AllRoles = allRoles
                };
                return View(model);
            }
            return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
