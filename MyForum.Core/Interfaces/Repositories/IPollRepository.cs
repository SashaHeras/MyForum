using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface IPollRepository : ITransientService, IRepository<Poll>
    {
        public IQueryable<Poll> GetAll();

        public Poll GetPollById(int id);

        public Poll GetPollByName(string name);
    }
}
