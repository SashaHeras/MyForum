using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyForum.Data.Dto.Post;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using MyForum.Extensions;
using System.Collections.Generic;

namespace MyForum.Controllers
{
    public class AdminController : Controller
    {
        private MyForumContext _context;
        private PostRepository _postRepository;
        private TopicRepository _topicRepository;
        private UserRepository _userRepository;
        private ComplaintRepository _complaintRepository;
        private CommentRepository _commentRepository;

        public AdminController(MyForumContext context)
        {
            _context = context;
            _topicRepository = new TopicRepository(_context);
            _userRepository = new UserRepository(_context);
            _postRepository = new PostRepository(_context);
            _commentRepository = new CommentRepository(_context);
            _complaintRepository = new ComplaintRepository(_context);
        }

        public IActionResult Main()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        public IActionResult CommentsList()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            ViewBag.Comments = _commentRepository.GetAll();

            return View();
        }

        public IActionResult PostsList()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            var posts = _postRepository.GetAll();

            List<AdminPost> adminPosts = new List<AdminPost>();

            foreach (var post in posts)
            {
                adminPosts.Add(new AdminPost()
                {
                    PostId = post.PostId,
                    PostName = post.PostName,
                    TopicId = post.TopicId,
                    Description = post.Description,
                    Updated = post.Updated.ToShortDateString(),
                    UserId = post.UserId,
                    UserName = _userRepository.GetUserNameById(post.UserId),
                    IsAllow = post.IsAllow,
                });
            }

            ViewBag.Posts = adminPosts; 

            return View();
        }

        public IActionResult TopicsList()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            ViewBag.Topics = _topicRepository.GetAll();

            return View();
        }

        public IActionResult ComplaintsList()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            ViewBag.Complaints = _complaintRepository.GetAll();

            return View();
        }

        public IActionResult UsersList()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            ViewBag.Users = _userRepository.GetAllWithoutUser(HttpContext.Session.Get<User>("user").Id);

            return View();
        }

        [Route("~/Admin/CreateUser")]
        public IActionResult CreateUser()
        {
            if(HttpContext.Session.Get<User>("user").IsAdmin == false)
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        [Route("~/Admin/Create")]
        public IActionResult CreateUser(User user)
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

            return RedirectToRoute(new { controller = "Admin", action = "UsersList" });
        }

        [Route("~/Admin/DeleteComplaiment/{id}")]
        public IActionResult DeleteComplaiment(int id)
        {
            _context.Complaint.Remove(_complaintRepository.GetComplaintById(id));
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Admin", action = "ComplaintsList" });
        }

        public bool CheckExist(string email)
        {
            if (_userRepository.GetUserNameByEmail(email) == null)
            {
                return false;
            }

            return true;
        }
    }
}
