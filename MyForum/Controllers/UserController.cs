using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using MyForum.ViewModels;
using MyForum.Controllers.Interfaces.Repositories;
using MyForum.Controllers.Data.Models;
using MyForum.Controllers.Data;
using MyForum.Controllers.Repository.Repositories;

namespace MyForum.Controllers.Repository
{
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _allUsers;
        private MyForumContext _context;
        private TopicRepository _topics;
        private UserRepository _users;

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
            if (checkExist(user.Email) == false)
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            if(checkPass(user.Email,user.Password) == false)
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            user = _users.GetAll().Where(u => u.Email.CompareTo(user.Email) == 0 && u.Password.CompareTo(user.Password) == 0).FirstOrDefault();

            HttpContext.Session.Set<User>("user", user);

            return RedirectToRoute(new { controller = "Home", action = "TopicsList"});
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
            if(user.Email.Length <= 0)
            {
                return RedirectToRoute(new { controller = "User", action = "Registration" });
            }

            if (checkExist(user.Email) == true)
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
        public IActionResult UsersList()
        {
            UsersListViewModel obj = new UsersListViewModel();
            var users = _allUsers.GetAll();
            ViewBag.AllUsers = users;

            return View(obj);
        }

        public bool checkExist(String email)
        {
            if(_users.GetUserNameByEmail(email) == null)
            {
                return false;
            }

            return true;
        }

        public bool checkPass(String email, String pass)
        {
            if(_users.GetAll().Where(u=>u.Email.CompareTo(email) == 0).FirstOrDefault().Password.CompareTo(pass) == 0)
            {
                return true;
            }

            return false;
        }
    }
}
