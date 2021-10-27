using MyForum.Controllers.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Interfaces.Repositories
{
    public interface ITopicRepository : IRepository<Topic>
    {
        public Topic GetTopicById(int id);

        public IQueryable<Topic> GetTopicByName(String name);
    }
}
