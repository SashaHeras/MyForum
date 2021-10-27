using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyForum.Controllers.Data;
using MyForum.Controllers.Interfaces.Repositories;
using MyForum.Controllers.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using MyForum.Models;
using MyForum.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers
{
    public class HomeController : Controller
    {
        private MyForumContext _context;
        private TopicRepository _topics;
        private UserRepository _users;
        private readonly ITopicRepository _allTopics;

        public HomeController(ITopicRepository topics, MyForumContext _context)
        {
            this._allTopics = topics;
            this._context = _context;
            this._topics = new TopicRepository(_context);
            this._users = new UserRepository(_context);
        }

        [HttpGet]
        public IActionResult TopicsList()
        {
            TopicsListViewModel topicView = new TopicsListViewModel();
            var topics = _allTopics.GetAll();

            ViewBag.AllTopics = topics;
            return View(topicView);
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
