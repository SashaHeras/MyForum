using Microsoft.AspNetCore.Mvc;
using MyForum.ViewModels;
using System.Linq;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Dto.Post;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using MyForum.Extensions;

namespace MyForum.Controllers
{
    public class PostController : Controller
    {
        private readonly MyForumContext _context;
        private readonly TopicRepository _topics;
        private readonly UserRepository _userRepository;
        private readonly ComentRepository _comentRepository;
        private static int _TopicId = -1;
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postrepos, MyForumContext context)
        {
            _postRepository = postrepos;
            _context = context;
            _topics = new TopicRepository(_context);
            _userRepository = new UserRepository(_context);
            _comentRepository = new ComentRepository(_context);
        }

        [Route("~/Post/PostsList/{id?}")]
        public IActionResult PostsList(int id)
        {
            PostsListViewModel obj = new();
            obj.AllPosts = _postRepository.GetPostsByTopicId(id);

            ViewData["TopicName"] = _topics.GetTopicNameById(id);

            ViewBag.Posts = obj.AllPosts;
            return View(obj);
        }

        [Route("~/Post/UserPosts/{id?}")]
        public IActionResult UserPosts(int id)
        {
            ViewBag.UserPosts = _postRepository.GetPostsByUserId(id);

            ViewBag.UsersName = _userRepository.GetUserById(id).Name;

            return View();
        }

        [HttpGet]
        [Route("~/Post/Post/{id?}")]
        public IActionResult Post(int id)
        {
            PostViewModel obj = new() {GetPostByTopicId = _postRepository.GetPostById(id)};

            FullPost post = new()
            {
                PostId = obj.GetPostByTopicId.PostId,
                PostName = obj.GetPostByTopicId.PostName,
                Description = obj.GetPostByTopicId.Description,
                PostUserId = obj.GetPostByTopicId.UserId,
                PostTopicId = obj.GetPostByTopicId.TopicId,
                PostUserName = _userRepository.GetUserNameById(obj.GetPostByTopicId.UserId)
            };

            if(_comentRepository.GetComentsByPostId(id) != null)
            {
                ViewBag.Comments = _comentRepository.GetComentsByPostId(id);
            }

            HttpContext.Session.Set("fullpost", post);

            ViewBag.Post = HttpContext.Session.Get<FullPost>("fullpost");

            return View(obj);
        }

        public IActionResult CreatePost()
        {
            ViewBag.TopicId = _topics.GetTopicByName(Request.Form["TopicName"]).FirstOrDefault().TopicId;

            ViewBag.User = HttpContext.Session.Get<User>("user");

            return View();
        }

        [Route("~/Post/Add")]
        public IActionResult Add(Post post)
        {
            if(post.Description != null && post.PostName != null)
            {
                _context.Post.Add(post);
                _context.SaveChanges();

                return RedirectToRoute(new { controller = "Post", action = "PostsList", id = post.TopicId });
            }

            return RedirectToRoute(new { controller = "Post", action = "PostsList", id = post.TopicId });
        }

        public IActionResult EditPosts()
        {
            ViewBag.Posts = _postRepository.GetPostsByTopicId(_topics.GetTopicByName(Request.Form["TopicName"].ToString()).FirstOrDefault().TopicId);
            ViewData["TopicName"] = Request.Form["TopicName"];

            return View();
        }

        [Route("~/Post/EditPost/{id?}")]
        public IActionResult EditPost(int id)
        {
            ViewBag.Post = _postRepository.GetPostById(id);

            return View();
        }

        [Route("~/Post/Edit")]
        public IActionResult Edit(Post post)
        {
            if(post.Description != null && post.PostName != null)
            {
                _context.Post.Update(post);
                _context.SaveChanges();

                return RedirectToRoute(new { controller = "Post", action = "PostsList", id = post.TopicId });
            }

            return RedirectToRoute(new { controller = "Post", action = "PostsList", id = post.TopicId });
        }

        public IActionResult DeletePost()
        {
            ViewBag.Posts = _postRepository.GetPostsByTopicId(_topics.GetTopicByName(Request.Form["TopicName"].ToString()).FirstOrDefault().TopicId);
            ViewData["TopicName"] = Request.Form["TopicName"];

            return View();
        }

        [Route("~/Post/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            bool isFind = false;
            Post post = new();

            int idInteger = id;
            foreach (var item in _context.Post)
            {
                if (item.PostId == idInteger)
                {
                    post = item;
                    isFind = true;
                    break;
                }
            }

            int topicId = 0;

            if (isFind)
            {
                topicId = _postRepository.GetPostById(id).TopicId;
                _context.Post.Remove(post);
                _context.SaveChanges();
            }

            return RedirectToRoute(new { controller = "Post", action = "PostsList", id = topicId });
        }
    }
}
