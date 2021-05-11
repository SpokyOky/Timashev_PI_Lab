using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timashev_PI_Lab;
using Timashev_PI_Lab.Logic;
using Timashev_PI_Lab.Models;

namespace Timashev_PI_Lab.Controllers
{
    public class UsersController : Controller
    {
        private readonly Database _context;
        private UserLogic _userLogic;

        public UsersController(UserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Login,Password,FIO")] User user)
        {
            if (ModelState.IsValid)
            {
                if (!_userLogic.CreateOrUpdate(user))
                {
                    ModelState.AddModelError("Error", "Такой пользователь уже существует");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user);
        }

        private bool UserExists(int? id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
