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
    public class PollRepository : Repository<Poll>, IPollRepository
    {
        public PollRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public Poll GetPollById(int id)
        {
            return GetAll().Where(p => p.Id == id).FirstOrDefault();
        }

        public Poll GetPollByName(string name)
        {
            return GetAll().Where(p => p.Name.Contains(name) || p.Name == name).FirstOrDefault();
        }
    }
}
