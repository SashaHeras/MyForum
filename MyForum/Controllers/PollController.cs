using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Core.Models;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using MyForum.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyForum.Controllers
{
    public class PollController : Controller
    {
        private MyForumContext _context;
        private IPollRepository _polls;
        private IQuestionRepository _questions;
        private IAnswerRepository _answers;

        public PollController(IPollRepository polls, MyForumContext context)
        {
            _polls = polls;
            _context = context;
            _questions = new QuestionRepository(context);
            _answers = new AnswerRepository(context);
        }

        [HttpGet]
        [Route("~/Poll/PollsList")]
        public IActionResult PollsList()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            if (HttpContext.Session.Get<User>("user") != null)
            {
                ViewBag.AllPolls = _polls.GetAll();

                return View();
            }

            return RedirectToRoute(new { controller = "User", action = "Login" });
        }

        [HttpGet]
        [Route("~/Poll/Poll/{id?}")]
        public IActionResult Poll(int id)
        {
            var poll = _polls.GetPollById(id);
            poll.CountViews++;

            var que = _questions.GetByPollId(id);

            foreach(PollQuestion q in que)
            {
                q.CountAnswers = _answers.CountAnswersOnQuestion(q.Id);

                _context.PollQuestions.Update(q);
            }

            ViewBag.Poll = poll;
            ViewBag.Questions = que;

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;
            ViewBag.UsersAnswer = GetQuestionId(HttpContext.Session.Get<User>("user").Id, que);
            ViewBag.UserId = HttpContext.Session.Get<User>("user").Id;

            _context.Polls.Update(poll);
            _context.SaveChanges();

            return View();
        }

        [HttpGet]
        [Route("~/Poll/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            ViewBag.UserId = HttpContext.Session.Get<User>("user").Id;

            ViewBag.Poll = _polls.GetPollById(id);

            ViewBag.Questions = _questions.GetByPollId(id);

            return View();
        }

        public IActionResult EditPoll(Poll poll)
        {
            var questions = _questions.GetByPollId(poll.Id);

            foreach(PollQuestion q in questions)
            {
                var text = Request.Form[q.Id.ToString()].ToString();

                if (text != null && text != "") 
                {
                    q.Name = text;
                }
                else
                {
                    return RedirectToRoute(new { controller = "Poll", action = "Edit", id = poll.Id });
                }

                _context.PollQuestions.Update(q);
                _context.SaveChanges();
            }

            _context.Polls.Update(poll);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Poll", action = "Poll", id = poll.Id });
        }

        [HttpGet]
        [Route("~/Poll/Delete/{id?}")]
        public IActionResult Delete(int id)
        {
            Poll p = _polls.GetPollById(id);

            IQueryable<PollQuestion> questions = _questions.GetByPollId(id);

            foreach(PollQuestion q in questions)
            {
                IQueryable<UserPollAnswer> answers = _answers.GetByQuestionId(q.Id);

                foreach(UserPollAnswer answer in answers)
                {
                    _context.UsersPollsAnswers.Remove(answer);
                    _context.SaveChanges();
                }

                _context.PollQuestions.Remove(q);
                _context.SaveChanges();
            }

            _context.Polls.Remove(p);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Poll", action = "PollsList" });
        }

        public IActionResult SetAnswer()
        {
            UserPollAnswer an = _answers.GetByUserIdAndPollId(Convert.ToInt32(Request.Form["UserId"]), Convert.ToInt32(Request.Form["PollId"]));

            if(an != null)
            {
                an.QuestionId = Convert.ToInt32(Request.Form["QuestionId"]);
                _context.UsersPollsAnswers.Update(an);
            }
            else
            {
                UserPollAnswer a = new UserPollAnswer()
                {
                    UserId = Convert.ToInt32(Request.Form["UserId"]),
                    QuestionId = Convert.ToInt32(Request.Form["QuestionId"])
                };

                _context.UsersPollsAnswers.Add(a);
            }

            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Poll", action = "Poll", id = Convert.ToInt32(Request.Form["PollId"]) });
        }

        [Route("~/Poll/SetCountQuestions")]
        public ActionResult SetCountQuestions()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Count = Request.Form["Count"];

            List<int> numbers = new List<int>(Convert.ToInt32(Request.Form["Count"]));

            for (int i = 1; i <= Convert.ToInt32(Request.Form["Count"]); i++) 
            {
                numbers.Add(i);
            }

            ViewBag.Numbers = numbers;

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        // POST: PollController/Create
        [HttpPost]
        [Route("~/Poll/CreatePoll")]
        public ActionResult CreatePoll(Poll poll)
        {
            Poll p = new Poll()
            {
                Name = poll.Name,
                Description = poll.Description,
                CountViews = 0,
                CountQuestions = Convert.ToInt32(Request.Form["Count"])
            };

            _context.Polls.Add(p);
            _context.SaveChanges();

            for (int i = 1; i <= Convert.ToInt32(Request.Form["Count"]); i++) 
            {
                PollQuestion question = new PollQuestion()
                {
                    Name = Request.Form[i.ToString()],
                    PollId = p.Id,
                    CountAnswers = 0
                };

                _context.PollQuestions.Add(question);
            }

            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Poll", action = "PollsList" });
        }

        public int GetQuestionId(int uid, IQueryable<PollQuestion> questions)
        {
            foreach(PollQuestion question in questions)
            {
                UserPollAnswer a = _answers.GetByQuestionId(question.Id).Where(a => a.UserId == uid).FirstOrDefault();
                if (a != null)
                {
                    return question.Id;
                }
            }

            return 0;
        }
    }
}
