using Microsoft.AspNetCore.Mvc;
using MyForum.Controllers.Data;
using MyForum.Controllers.Data.Models;
using MyForum.Controllers.Dto.Post;
using MyForum.Controllers.Interfaces.Repositories;
using MyForum.Controllers.Repository.Repositories;
using MyForum.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers
{
    public class PostController : Controller
    {
        private MyForumContext _context;
        private TopicRepository _topics;
        private UserRepository _userRepository;
        private ComentRepository _comentRepository;
        private static Int32 _TopicId = -1;
        private readonly IPostRepository _allPosts;

        public PostController(IPostRepository postrepos, MyForumContext context)
        {
            _allPosts = postrepos;
            _context = context;
            _topics = new TopicRepository(_context);
            _userRepository = new UserRepository(_context);
            _comentRepository = new ComentRepository(_context);
        }

        [Route("~/Post/PostsList/{id?}")]
        public IActionResult PostsList(Int32 id)
        {
            PostsListViewModel obj = new PostsListViewModel();
            obj.AllPosts = _allPosts.GetPostsByTopicId(id);

            ViewData["TopicName"] = _topics.GetTopicNameById(id);

            ViewBag.Posts = obj.AllPosts;
            return View(obj);
        }

        [Route("~/Post/UserPosts/{id?}")]
        public IActionResult UserPosts(Int32 id)
        {
            ViewBag.UserPosts = _allPosts.GetPostsByUserId(id);

            ViewBag.UsersName = _userRepository.GetUserById(id).Name;

            return View();
        }

        [HttpGet]
        [Route("~/Post/Post/{id?}")]
        public IActionResult Post(Int32 id)
        {
            PostViewModel obj = new PostViewModel();
            obj.GetPostByTopicId = _allPosts.GetPostById(id);

            FullPost post = new FullPost();

            post.PostId = obj.GetPostByTopicId.PostId;
            post.PostName = obj.GetPostByTopicId.PostName;
            post.Description = obj.GetPostByTopicId.Description;
            post.PostUserId = obj.GetPostByTopicId.UserId;
            post.PostTopicId = obj.GetPostByTopicId.TopicId;

            post.PostUserName = _userRepository.GetUserNameById(obj.GetPostByTopicId.UserId);

            if(_comentRepository.GetComentsByPostId(id) != null)
            {
                ViewBag.Comments = _comentRepository.GetComentsByPostId(id);
            }

            HttpContext.Session.Set<FullPost>("fullpost", post);

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
            ViewBag.Posts = _allPosts.GetPostsByTopicId(_topics.GetTopicByName(Request.Form["TopicName"].ToString()).FirstOrDefault().TopicId);
            ViewData["TopicName"] = Request.Form["TopicName"];

            return View();
        }

        [Route("~/Post/EditPost/{id?}")]
        public IActionResult EditPost(Int32 id)
        {
            ViewBag.Post = _allPosts.GetPostById(id);

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
            ViewBag.Posts = _allPosts.GetPostsByTopicId(_topics.GetTopicByName(Request.Form["TopicName"].ToString()).FirstOrDefault().TopicId);
            ViewData["TopicName"] = Request.Form["TopicName"];

            return View();
        }

        [Route("~/Post/Delete/{id}")]
        public IActionResult Delete(Int32 id)
        {
            bool isFind = false;
            Post post = new Post();

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
                topicId = _allPosts.GetPostById(id).TopicId;
                _context.Post.Remove(post);
                _context.SaveChanges();
            }

            return RedirectToRoute(new { controller = "Post", action = "PostsList", id = topicId });
        }
    }
}
