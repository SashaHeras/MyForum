using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Core.Models;
using MyForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface IAnswerRepository : ITransientService, IRepository<UserPollAnswer>
    {
        public IQueryable<UserPollAnswer> GetAll();

        public UserPollAnswer GetById(int id);

        public int CountAnswersOnQuestion(int id);

        public UserPollAnswer GetByUserIdAndPollId(int uid, int pid);

        public IQueryable<UserPollAnswer> GetByQuestionId(int id);
    }
}
