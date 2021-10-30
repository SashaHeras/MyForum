﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using MyForum.Extensions;
using MyForum.ViewModels;

namespace MyForum.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _allUsers;
        private readonly MyForumContext _context;
        private TopicRepository _topics;
        private readonly UserRepository _users;

        public UserController(IUserRepository iAllUsers, MyForumContext context)
        {
            _allUsers = iAllUsers;
            _context = context;
            _topics = new TopicRepository(_context);
            _users = new UserRepository(_context);
        }

        // Метод перехода на страницу для входа
        [HttpGet]
        [Route("~/")]
        [Route("~/User/Login")]
        public IActionResult Login()
        {
            return View();
        }

        // Метод 
        [Route("~/User/LoginUser")]
        public IActionResult LoginUser(User user)
        {
            if (CheckExist(user.Email) == false)
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            if (CheckPass(user.Email, user.Password) == false)
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            user = _users.GetAll().FirstOrDefault(u => 
                    string.Compare(u.Email, user.Email, StringComparison.Ordinal) == 0 
                    && 
                    string.Compare(u.Password, user.Password, StringComparison.Ordinal) == 0);

            HttpContext.Session.Set("user", user);

            ViewBag.UserLogined = user;

            return RedirectToRoute(new { controller = "Home", action = "TopicsList" });
        }

        [HttpGet]
        [Route("~/User/Registration")]
        public IActionResult Registration()
        {
            return View();
        }

        // Метод 
        [Route("~/User/RegistrationUser")]
        public IActionResult RegistrationUser(User user)
        {
            if (user.Email.Length <= 0)
            {
                return RedirectToRoute(new { controller = "User", action = "Registration" });
            }

            if (CheckExist(user.Email) == true)
            {
                return RedirectToRoute(new { controller = "User", action = "Registration" });
            }

            if (user.Password.Length < 8 || user.Password == null)
            {
                return RedirectToRoute(new { controller = "User", action = "Registration" });
            }

            if (user.Address.Length <= 0 || user.Address == null)
            {
                return RedirectToRoute(new { controller = "User", action = "Registration" });
            }

            if (user.Age <= 0)
            {
                return RedirectToRoute(new { controller = "User", action = "Registration" });
            }

            _context.User.Add(user);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "User", action = "Login" });
        }

        [HttpGet]
        [Route("~/User/Profile")]
        public IActionResult Profile()
        {
            if (HttpContext.Session.Keys.Contains("user") != true)
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            ViewBag.User = HttpContext.Session.Get<User>("user");

            return View();
        }

        [HttpGet]
        public IActionResult UsersList()
        {
            UsersListViewModel obj = new();
            var users = _allUsers.GetAll();
            ViewBag.AllUsers = users;

            return View(obj);
        }

        [HttpGet]
        [Route("~/User/ChangePass")]
        public IActionResult ChangePass()
        {
            return View();
        }

        [Route("~/User/SentNewPass")]
        public IActionResult SentNewPass(ChangePassViewModel pass)
        {
            if(String.Compare(pass.oldPass, HttpContext.Session.Get<User>("user").Password, StringComparison.Ordinal) != 0)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangePass" });
            }

            if (pass.newPass.Length <= 8)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangePass" });
            }

            if(pass.newPassConfign.Length <= 8 || pass.newPassConfign.Length != pass.newPass.Length)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangePass" });
            }

            if (pass.newPass.CompareTo(pass.newPassConfign) != 0) 
            {
                return RedirectToRoute(new { controller = "User", action = "ChangePass" });
            }

            if(pass.oldPass.CompareTo(pass.newPass) == 0)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangePass" });
            }

            User user = HttpContext.Session.Get<User>("user");

            user.Password = pass.newPass;

            _context.User.Update(user);
            _context.SaveChanges();

            HttpContext.Session.Remove("user");
            HttpContext.Session.Set("user", user);

            return RedirectToRoute(new { controller = "User", action = "Profile" });
        }

        [HttpGet]
        [Route("~/User/ChangeOwnData")]
        public IActionResult ChangeOwnData()
        {
            ViewBag.UserData = HttpContext.Session.Get<User>("user");

            return View();
        }

        [Route("~/User/SentNewData")]
        public IActionResult SentNewData(EditUserViewModel newUser)
        {
            if(newUser.Name.Length == 0 || newUser.Surname.Length == 0 || newUser.Age < 0 || newUser.Email.Length == 0 || newUser.Address.Length == 0)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangeData" });
            }

            if(newUser.Email.Contains('@') == false)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangeData" });
            }

            User user = HttpContext.Session.Get<User>("user");

            user.Name = newUser.Name;
            user.Surname = newUser.Surname;
            user.Age = newUser.Age;
            user.Email = newUser.Email;
            user.Address = newUser.Address;

            _context.User.Update(user);
            _context.SaveChanges();

            HttpContext.Session.Remove("user");
            HttpContext.Session.Set("user", user);

            return RedirectToRoute(new { controller = "User", action = "Profile" });
        }


        public bool CheckExist(string email)
        {
            if(_users.GetUserNameByEmail(email) == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckPass(string email, string pass)
        {
            if(_users.GetAll().Where(u=>u.Email.CompareTo(email) == 0).FirstOrDefault().Password.CompareTo(pass) == 0)
            {
                return true;
            }

            return false;
        }
    }
}
