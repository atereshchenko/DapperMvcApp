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
        IUserRepository repo;
        public UserController(IUserRepository r)
        {
            repo = r;
        }

        public ActionResult Index()
        {
            return View(repo.GetUsers());
        }

        public ActionResult Details(int id)
        {
            User user = repo.Get(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            repo.Create(user);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            User user = repo.Get(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            repo.Update(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            User user = repo.Get(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
