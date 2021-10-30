using Microsoft.AspNetCore.Mvc;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Dto.Post;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using MyForum.Extensions;

namespace MyForum.Controllers
{
    public class ComentController : Controller
    {
        private readonly MyForumContext _context;
        private TopicRepository _topics;
        private PostRepository _postRepository;
        private UserRepository _userRepository;
        private readonly IComentRepository _comentRepository;

        public ComentController(IComentRepository coments, MyForumContext _context)
        {
            _comentRepository = coments;
            this._context = _context;
            _topics = new TopicRepository(_context);
            _userRepository = new UserRepository(_context);
            _postRepository = new PostRepository(_context);
        }

        public IActionResult CreateComent()
        {
            ViewBag.Post = HttpContext.Session.Get<FullPost>("fullpost");

            ViewBag.User = HttpContext.Session.Get<User>("user");

            return View();
        }

        [Route("~/Coment/Add")]
        public IActionResult Add(Coment coment)
        {
            if (string.IsNullOrEmpty(coment.Comment))
                return RedirectToRoute(new {controller = "Coment", action = "CreateComent"});
            
            _context.Coment.Add(coment);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Post", action = "Post", id = coment.PostId });

        }
    }
}
