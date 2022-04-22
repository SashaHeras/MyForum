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
    public class QuestionRepository : Repository<PollQuestion>, IQuestionRepository
    {
        public QuestionRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public PollQuestion GetQuestionById(int id)
        {
            return GetAll().Where(q => q.Id == id).FirstOrDefault();
        }

        public IQueryable<PollQuestion> GetByPollId(int pollId)
        {
            return GetAll().Where(q => q.PollId == pollId);
        }
    }
}
