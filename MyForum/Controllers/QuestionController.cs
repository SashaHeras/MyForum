using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Core.Models;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using System.Linq;

namespace MyForum.Controllers
{
    public class QuestionController : Controller
    {
        private MyForumContext _context;
        private IPollRepository _polls;
        private IQuestionRepository _questions;
        private IAnswerRepository _answers;

        public QuestionController(IQuestionRepository questions, MyForumContext context)
        {
            _questions = questions;
            _context = context;
            _polls = new PollRepository(context);
            _answers = new AnswerRepository(context);
        }

        [HttpGet]
        [Route("~/Question/Delete/{id?}/{eqid?}")]
        public IActionResult Delete(int id, int eqid)
        {
            PollQuestion q = _questions.GetQuestionById(id);

            IQueryable<UserPollAnswer> answers = _answers.GetByQuestionId(id);

            foreach (UserPollAnswer answer in answers)
            {
                _context.UsersPollsAnswers.Remove(answer);
                _context.SaveChanges();
            }

            _context.PollQuestions.Remove(q);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Poll", action = "Edit", id = eqid });
        }

        [HttpGet]
        [Route("~/Question/Add/{id?}")]
        public IActionResult Add(int id)
        {
            PollQuestion q = new PollQuestion()
            {
                Name = "",
                PollId = id,
                CountAnswers = 0
            };

            _context.PollQuestions.Add(q);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Poll", action = "Edit", id = id });
        }
    }
}
