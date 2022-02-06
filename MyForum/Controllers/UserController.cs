using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private MyForumContext _context;
        private PostRepository _postRepository;
        private TopicRepository _topics;
        private MarkRepository _markRepository;
        private readonly UserRepository _userRepository;

        public UserController(IUserRepository iAllUsers, MyForumContext context)
        {
            _allUsers = iAllUsers;
            _context = context;
            _topics = new TopicRepository(_context);
            _userRepository = new UserRepository(_context);
            _postRepository = new PostRepository(_context);
            _markRepository = new MarkRepository(_context);
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
            if (HttpContext.Session.Keys.Contains("user") == true)
            {
                HttpContext.Session.Remove("user");
            }

            if(!Regex.IsMatch(user.Email,@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}"))
            {
                ModelState.AddModelError("Email", "Uncorrect email");

                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            if (CheckExist(user.Email) == false)
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            if (CheckPass(user.Email, user.Password) == false || user.Password == null)
            {
                ModelState.AddModelError("Password", "Uncorrect password");

                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            user = _userRepository.GetAll().FirstOrDefault(u => 
                    string.Compare(u.Email, user.Email) == 0 
                    && 
                    string.Compare(u.Password, user.Password) == 0);

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

        [Route("~/User/UserPosts/{id?}")]
        public IActionResult UserPosts(int id)
        {
            ViewBag.UserPosts = _postRepository.GetPostsByUserId(id);

            ViewBag.UsersName = _userRepository.GetUserById(id).Name;

            ViewBag.AverageMark = averageMark(id);

            return View();
        }

        private float averageMark(int id)
        {
            int postsCount = _postRepository.GetPostsByUserId(id).Count();
            int allSum = 0;

            var posts = _postRepository.GetPostsByUserId(id);

            foreach (var i in posts)
            {
                allSum += _markRepository.GetPostMark(i.PostId);
            }

            float average = allSum / postsCount;

            return average;
        }

        public bool CheckExist(string email)
        {
            if(_userRepository.GetUserNameByEmail(email) == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckPass(string email, string pass)
        {
            if (_userRepository.GetAll().Where(u => u.Email.CompareTo(email) == 0).FirstOrDefault().Password.CompareTo(pass) == 0) 
            {
                return true;
            }

            return false;
        }
    }
}
