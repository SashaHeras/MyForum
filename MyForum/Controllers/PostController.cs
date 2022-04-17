using Microsoft.AspNetCore.Mvc;
using MyForum.ViewModels;
using System.Linq;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Dto.Post;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using MyForum.Extensions;
using MyForum.Core.Models;
using System;
using System.Threading.Tasks;
using System.Web.WebPages.Html;
using System.Net;

namespace MyForum.Controllers
{
    public class PostController : Controller
    {
        private readonly MyForumContext _context;
        private readonly TopicRepository _topics;
        private readonly UserRepository _userRepository;
        private readonly CommentRepository _comentRepository;
        private readonly MarkRepository _markRepository;
        private ComplaintRepository _complaintRepository;
        private static int _TopicId = -1;
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postrepos, MyForumContext context)
        {
            _postRepository = postrepos;
            _context = context;
            _topics = new TopicRepository(_context);
            _userRepository = new UserRepository(_context);
            _comentRepository = new CommentRepository(_context);
            _markRepository = new MarkRepository(_context);
            _complaintRepository = new ComplaintRepository(_context);
        }

        [Route("~/Post/PostsList/{id?}")]
        public IActionResult PostsList(int id)
        {
            if(HttpContext.Session.Keys.Contains("post") == true)
            {
                HttpContext.Session.Remove("post");
            }

            PostsListViewModel obj = new();
            obj.AllPosts = _postRepository.GetPostsByTopicId(id);

            ViewData["TopicName"] = _topics.GetTopicNameById(id);

            if(HttpContext.Session.Get<int>("sort") == 0)
            {
                ViewBag.Posts = obj.AllPosts.Where(p => p.IsAllow == true);
                ViewBag.SortType = 0;
            }
            else
            {
                if(HttpContext.Session.Get<int>("sort") == 1)
                {
                    ViewBag.Posts = obj.AllPosts.Where(p => p.IsAllow == true).OrderByDescending(p => p.Views);
                    ViewBag.SortType = 1;
                }
                else if(HttpContext.Session.Get<int>("sort") == 2)
                {
                    ViewBag.Posts = obj.AllPosts.Where(p => p.IsAllow == true).OrderByDescending(p => p.Updated);
                    ViewBag.SortType = 2;
                }
            }

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View(obj);
        }

        [Route("~/Post/CategoryChosen")]
        public IActionResult ChoosenSortType(SelectListItem item)
        {
            HttpContext.Session.Set<int>("sort", Convert.ToInt32(Request.Form["SortType"]));

            return RedirectToRoute(new { controller = "Post", action = "PostsList", id = _topics.GetTopicByName(Request.Form["topic"].ToString()).FirstOrDefault().TopicId }) ;
        }

        [Route("~/Post/SearchPosts")]
        public IActionResult SearchPosts()
        {
            if (HttpContext.Session.Keys.Contains("post") == true)
            {
                HttpContext.Session.Remove("post");
            }

            ViewBag.Search = Request.Form["Search"].ToString();

            ViewBag.Posts = _postRepository.GetAll().Where(p => p.IsAllow == true && (p.PostName.Contains(Request.Form["Search"].ToString()) || p.Description.Contains(Request.Form["Search"].ToString())));

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        [HttpGet]
        [Route("~/Post/Post/{id?}")]
        public IActionResult Post(int id)
        {
            PostViewModel obj = new() {GetPostByTopicId = _postRepository.GetPostById(id)};

            FullPost post = new FullPost(obj.GetPostByTopicId.PostId, obj.GetPostByTopicId.PostName, obj.GetPostByTopicId.Description, obj.GetPostByTopicId.UserId, obj.GetPostByTopicId.TopicId, _userRepository.GetUserNameById(obj.GetPostByTopicId.UserId), (int)_markRepository.GetAll().Where(mark => mark.PostId == obj.GetPostByTopicId.PostId).Sum(mark => mark.PostMark));
            post.Updated = _postRepository.GetPostById(id).Updated.Date.ToShortDateString();
            post.Views = _postRepository.GetPostById(id).Views;

            Post p = _postRepository.GetPostById(id);
            p.Views++;

            _context.Post.Update(p);
            _context.SaveChanges();

            if (_comentRepository.GetCommentsByPostId(id) != null)
            {
                ViewBag.Comments = _comentRepository.GetCommentsByPostId(id).Where(c => c.IsAllow == true);
            }

            HttpContext.Session.Set("fullpost", post);

            ViewBag.Post = HttpContext.Session.Get<FullPost>("fullpost");
            ViewBag.User = HttpContext.Session.Get<User>("user");

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View(obj);
        }

        public IActionResult CreatePost()
        {
            ViewBag.TopicId = _topics.GetTopicByName(Request.Form["TopicName"]).FirstOrDefault().TopicId;

            ViewBag.User = HttpContext.Session.Get<User>("user");

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        [Route("~/Post/Add")]
        public IActionResult Add(Post post)
        {
            if(post.Description != null && post.PostName != null)
            {
                post.Updated = DateTime.Now;
                _postRepository.AddAsync(post);

                return RedirectToRoute(new { controller = "Post", action = "PostsList", id = post.TopicId });
            }

            return RedirectToRoute(new { controller = "Post", action = "PostsList", id = post.TopicId });
        }

        public IActionResult EditPosts()
        {
            ViewBag.Posts = _postRepository.GetPostsByTopicId(_topics.GetTopicByName(Request.Form["TopicName"].ToString()).FirstOrDefault().TopicId);
            ViewData["TopicName"] = Request.Form["TopicName"];
            ViewBag.TopicId = _topics.GetTopicByName(Request.Form["TopicName"].ToString()).FirstOrDefault().TopicId;

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        [Route("~/Post/EditPost/{id?}")]
        public IActionResult EditPost(int id)
        {
            if (id == 0)
            {
                ViewBag.Post = _postRepository.GetPostById(Convert.ToInt32(Request.Form["PostId"]));
            }
            else
            {
                ViewBag.Post = _postRepository.GetPostById(id);
            }

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        [Route("~/Post/Edit")]
        public IActionResult EditPost(Post post)
        {
            if(string.IsNullOrEmpty(post.Description))
            {
                ModelState.AddModelError("Description", "Uncorrect discription!");

                ViewBag.Post = post;
            }

            if (string.IsNullOrEmpty(post.PostName))
            {
                ModelState.AddModelError("PostName", "Uncorrect post name!");

                ViewBag.Post = post;
            }

            if (ModelState.IsValid)
            {
                post.Updated = DateTime.Now;

                _context.Post.Update(post);
                _context.SaveChanges();

                return RedirectToRoute(new { controller = "Post", action = "PostsList", id = post.TopicId });
            }

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        public IActionResult DeletePost()
        {
            ViewBag.Posts = _postRepository.GetPostsByTopicId(_topics.GetTopicByName(Request.Form["TopicName"].ToString()).FirstOrDefault().TopicId);
            ViewData["TopicName"] = Request.Form["TopicName"];
            ViewBag.TopicId = _topics.GetTopicByName(Request.Form["TopicName"].ToString()).FirstOrDefault().TopicId;

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        [Route("~/Post/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Post post = _postRepository.GetPostById(Convert.ToInt32(Request.Form["PostId"]));

            // Save id of post`s topic to redirect user on the page of all posts after removing
            int topicId = _postRepository.GetPostById(Convert.ToInt32(Request.Form["PostId"])).TopicId;

            _context.Post.Remove(post);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Post", action = "PostsList", id = topicId });
        }

        [Route("~/Post/CreateComplaint")]
        public IActionResult CreateComplaint()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            ViewBag.PostId = Request.Form["PostId"];

            return View();
        }

        [Route("~/Post/ComplaintCreate")]
        public IActionResult Create(Complaint comp)
        {
            _context.Complaint.Add(comp);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Post", action = "Post", id = comp.PostId });
        }

        public IActionResult Delete()
        {
            Post post = _postRepository.GetPostById(Convert.ToInt32(Request.Form["PostId"]));

            // Save id of post`s topic to redirect user on the page of all posts after removing
            int topicId = _postRepository.GetPostById(Convert.ToInt32(Request.Form["PostId"])).TopicId;

            _context.Post.Remove(post);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Post", action = "PostsList", id = topicId });
        }

        [Route("~/Post/Disallow/{id}")]
        public IActionResult Disallow(int id)
        {
            _postRepository.GetPostById(id).IsAllow = false;

            _context.Post.Update(_postRepository.GetPostById(id));
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Admin", action = "PostsList" });
        }

        [Route("~/Post/Allow/{id}")]
        public IActionResult Allow(int id)
        {
            _postRepository.GetPostById(id).IsAllow = true;

            _context.Post.Update(_postRepository.GetPostById(id));
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Admin", action = "PostsList" });
        }

        [Route("~/Post/DeleteById/{id}")]
        public IActionResult DeleteById(int id)
        {
            Post post = _postRepository.GetPostById(id);

            _context.Post.Remove(post);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Admin", action = "PostsList" });
        }
    }
}
