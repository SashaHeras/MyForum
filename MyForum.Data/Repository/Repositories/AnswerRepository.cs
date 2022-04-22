using MyForum.Core.Interfaces.Repositories;
using MyForum.Core.Models;
using MyForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Data.Repository.Repositories
{
    public class AnswerRepository : Repository<UserPollAnswer>, IAnswerRepository
    {
        MyForumContext _context;

        public AnswerRepository(MyForumContext forumContext) : base(forumContext)
        {
            _context = forumContext;
        }

        public int CountAnswersOnQuestion(int id)
        {
            return GetAll().Where(a => a.QuestionId == id).Count();
        }

        public UserPollAnswer GetById(int id)
        {
            return GetAll().Where(a => a.Id == id).FirstOrDefault();
        }

        public IQueryable<UserPollAnswer> GetByQuestionId(int id)
        {
            return GetAll().Where(a => a.QuestionId == id);
        }

        public UserPollAnswer GetByUserIdAndPollId(int uid, int pid)
        {
            var que = _context.PollQuestions.Where(q => q.PollId == pid);

            foreach(PollQuestion q in que)
            {
                if (GetAll().Where(a => a.UserId == uid && a.QuestionId == q.Id).FirstOrDefault() != null) 
                {
                    return GetAll().Where(a => a.UserId == uid && a.QuestionId == q.Id).FirstOrDefault();
                }
            }

            return null;
        }
    }
}
