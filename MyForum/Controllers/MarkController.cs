using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Core.Models;
using MyForum.Data.Dto.Post;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using MyForum.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers
{
    public class MarkController : Controller
    {
        private readonly MyForumContext _context;
        private TopicRepository _topics;
        private PostRepository _postRepository;
        private UserRepository _userRepository;
        private readonly IMarkRepository _markRepository;

        public MarkController(IMarkRepository _markRepository, MyForumContext _context)
        {
            this._context = _context;
            _topics = new TopicRepository(_context);
            _userRepository = new UserRepository(_context);
            _postRepository = new PostRepository(_context);
            this._markRepository = new MarkRepository(_context);
        }

        [Route("~/Mark/IncreseMark/")]
        public IActionResult IncreseMark()
        {
            return SetMark(true);
        }

        [Route("~/Mark/DegreseMark/")]
        public IActionResult DegreseMark()
        {
            return SetMark(false);
        }

        public IActionResult SetMark(bool isPositive)
        {
            FullPost post = HttpContext.Session.Get<FullPost>("fullpost");
            User u = HttpContext.Session.Get<User>("user");

            var mark = _markRepository.GetAll().FirstOrDefault(mark => mark.PostId == post.PostId && mark.UserId == u.Id);

            if (mark == null)
            {
                UserPostMark m = new()
                {
                    UserId = u.Id,
                    PostId = post.PostId,
                    PostMark = isPositive == true ? 1 : -1
                };

                _ = _markRepository.AddAsync(m);

                return RedirectToRoute(new { controller = "Post", action = "Post", id = post.PostId });
            }

            mark.PostMark = isPositive == true ? 1 : -1;

            _ = _markRepository.UpdateAsync(mark);
            return RedirectToRoute(new { controller = "Post", action = "Post", id = post.PostId });
        }
    }
}
