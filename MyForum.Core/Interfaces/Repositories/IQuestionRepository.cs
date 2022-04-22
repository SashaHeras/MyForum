using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface IQuestionRepository : ITransientService, IRepository<PollQuestion>
    {
        public IQueryable<PollQuestion> GetAll();

        public PollQuestion GetQuestionById(int id);

        public IQueryable<PollQuestion> GetByPollId(int pollId);
    }
}
