using Microsoft.AspNetCore.Mvc;
using MyForum.Controllers.Data;
using MyForum.Controllers.Data.Models;
using MyForum.Controllers.Interfaces.Repositories;
using MyForum.Controllers.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers
{
    public class ComentController : Controller
    {
        private MyForumContext _context;
        private TopicRepository _topics;
        private PostRepository _postRepository;
        private UserRepository _userRepository;
        private readonly IComentRepository _comentRepository;

        public ComentController(IComentRepository coments, MyForumContext _context)
        {
            this._comentRepository = coments;
            this._context = _context;
            this._topics = new TopicRepository(_context);
            this._userRepository = new UserRepository(_context);
            this._postRepository = new PostRepository(_context);
        }

        public IActionResult CreateComent()
        {
            ViewBag.Post = HttpContext.Session.Get<Post>("post");
            ViewData["UserName"] = _userRepository.GetUserNameById(HttpContext.Session.Get<Post>("post").UserId);

            ViewBag.UserId = HttpContext.Session.Get<User>("user").Id;

            return View();
        }
    }
}
