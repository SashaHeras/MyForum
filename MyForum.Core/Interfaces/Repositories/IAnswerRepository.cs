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

        public UserPollAnswer Update(int id, int nqid);

        public IQueryable<UserPollAnswer> GetByQuestionId(int id);
    }
}
