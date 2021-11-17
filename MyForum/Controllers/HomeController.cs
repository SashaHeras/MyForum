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
        private UserRepository _users;
        private readonly ITopicRepository _allTopics;

        public HomeController(ITopicRepository topics, MyForumContext context)
        {
            _allTopics = topics;
            _context = context;
            _topics = new TopicRepository(context);
            _users = new UserRepository(context);
        }

        [HttpGet]
        public IActionResult TopicsList()
        {
            if(HttpContext.Session.Get<User>("user") != null)
            {
                TopicsListViewModel topicView = new();
                var topics = _allTopics.GetAll();

                ViewBag.AllTopics = topics;

                return View(topicView);
            }

            return RedirectToRoute(new { controller = "User", action = "Login" });
        } 

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
