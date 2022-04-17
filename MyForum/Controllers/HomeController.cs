using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyForum.ViewModels;
using System;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using MyForum.Extensions;

namespace MyForum.Controllers
{
    public class HomeController : Controller
    {
        private MyForumContext _context;
        private TopicRepository _topics;
        private PostRepository _postRepository;
        private UserRepository _users;
        private readonly ITopicRepository _allTopics;

        public HomeController(ITopicRepository topics, MyForumContext context)
        {
            _allTopics = topics;
            _context = context;
            _topics = new TopicRepository(context);
            _users = new UserRepository(context);
            _postRepository = new PostRepository(context);
        }

        [HttpGet]
        public IActionResult TopicsList()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            if (HttpContext.Session.Get<User>("user") != null)
            {
                TopicsListViewModel topicView = new();

                ViewBag.AllTopics = _allTopics.GetAllowedTopics();
                ViewBag.PopularPosts = _postRepository.GetPopularAllowedPosts();

                return View(topicView);
            }

            return RedirectToRoute(new { controller = "User", action = "Login" });
        }

        [Route("~/Home/SearchTopic")]
        public IActionResult SearchTopic()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            if (HttpContext.Session.Get<User>("user") != null)
            {
                TopicsListViewModel topicView = new();

                ViewBag.Search = Request.Form["SearchTopic"].ToString();

                ViewBag.Topics = _allTopics.SearchAllowedTopics(Request.Form["SearchTopic"].ToString());

                return View(topicView);
            }

            return RedirectToRoute(new { controller = "User", action = "Login" });
        }

        [Route("~/Home/CreateTopic")]
        public IActionResult CreateTopic()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        public IActionResult Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                _topics.AddAsync(topic);

                return RedirectToRoute(new { controller = "Home", action = "TopicsList" });
            }

            return RedirectToRoute(new { controller = "Home", action = "CreateTopic" });
        }

        [Route("~/Topic/Disallow/{id}")]
        public IActionResult Disallow(int id)
        {
            _topics.GetTopicById(id).IsAllow = false;

            _context.Topic.Update(_topics.GetTopicById(id));
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Admin", action = "TopicsList" });
        }

        [Route("~/Topic/Allow/{id}")]
        public IActionResult Allow(int id)
        {
            _topics.GetTopicById(id).IsAllow = true;

            _context.Topic.Update(_topics.GetTopicById(id));
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Admin", action = "TopicsList" });
        }

        [Route("~/Topic/Delete/{id}")]
        public IActionResult DeleteById(int id)
        {
            Topic topic = _topics.GetTopicById(id);

            _context.Topic.Remove(topic);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Admin", action = "TopicsList" });
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            
            
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
