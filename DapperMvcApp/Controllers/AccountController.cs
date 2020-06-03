using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DapperMvcApp.ViewModels; 
using DapperMvcApp.Models; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using DapperMvcApp.Models.Services;
using DapperMvcApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace DapperMvcApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private IUserRepository _user;

        public AccountController(ILogger<AccountController> logger, IUserRepository user)
        {
            _logger = logger;
            _user = user;
        }

        [AllowAnonymous]
        [HttpGet]        
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _user.Get(model.Email, model.Password);
                if (user != null)
                {
                    await Authenticate(user.Name);
                    if (model.ReturnUrl != null)
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Пользователь с указанным логином и паролем не найден");
            }
            else
            {
                ModelState.AddModelError("", "Ошибка валидации");
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]        
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _user.FindByEmail(model.Email);
                if (user == null)
                {
                    await _user.Create(model.Email, model.Password);
                    await Authenticate(user.Email);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }           
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task Authenticate(string userName)
        {
            // создаем claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
