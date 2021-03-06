using Microsoft.AspNetCore.Mvc;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Dto.Post;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using MyForum.Extensions;

namespace MyForum.Controllers
{
    public class CommentController : Controller
    {
        private readonly MyForumContext _context;
        private TopicRepository _topics;
        private PostRepository _postRepository;
        private UserRepository _userRepository;
        private readonly ICommentRepository _comentRepository;

        public CommentController(ICommentRepository coments, MyForumContext _context)
        {
            _comentRepository = coments;
            this._context = _context;
            _topics = new TopicRepository(_context);
            _userRepository = new UserRepository(_context);
            _postRepository = new PostRepository(_context);
        }

        [Route("~/Comment/CreateComment")]
        public IActionResult CreateComment()
        {
            ViewBag.Post = HttpContext.Session.Get<FullPost>("fullpost");

            ViewBag.User = HttpContext.Session.Get<User>("user");

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        [Route("~/Comment/Disallow/{id}")]
        public IActionResult Disallow(int id)
        {
            _comentRepository.GetCommentById(id).IsAllow = false;
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Admin", action = "CommentsList" });
        }

        [Route("~/Comment/Allow/{id}")]
        public IActionResult Allow(int id)
        {
            _comentRepository.GetCommentById(id).IsAllow = true;
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Admin", action = "CommentsList" });
        }

        [Route("~/Comment/Add")]
        public IActionResult Add(Comment Comment)
        {
            if (string.IsNullOrEmpty(Comment.CommentText))
                return RedirectToRoute(new {controller = "Comment", action = "CreateComment" });
            
            _context.Comment.Add(Comment);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Post", action = "Post", id = Comment.PostId });
        }

        [Route("~/Comment/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _context.Comment.Remove(_comentRepository.GetCommentById(id));
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Admin", action = "CommentsList" });
        }
    }
}
