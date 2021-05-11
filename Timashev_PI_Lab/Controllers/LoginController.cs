using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timashev_PI_Lab.Logic;
using Timashev_PI_Lab.Models;
using Microsoft.AspNetCore.Mvc;

namespace Timashev_PI_Lab.Controllers
{
    public class LoginController : Controller
    {
        private UserLogic _userLogic;

        public LoginController(UserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var _user = _userLogic.Read(user).FirstOrDefault();

            if (_user == null)
            {
                ModelState.AddModelError("Ошибка", "Пользователь не найден или неверный пароль");

                return View("index", user);
            }
            else
            {
                Program.User = _user;
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Logout()
        {
            Program.User = null;
            return RedirectToAction("Index", "Login");
        }
    }
}
