using MyForum.Controllers.Data;
using MyForum.Controllers.Data.Models;
using MyForum.Controllers.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Repository.Repositories
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public TopicRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public Topic GetTopicById(int id)
        {
            return GetAll().Where(t => t.TopicId == id).FirstOrDefault();
        }

        public IQueryable<Topic> GetTopicByName(String name)
        {
            return GetAll().Where(t => t.TopicName.Contains(name));
        }

        public String GetTopicNameById(int id)
        {
            return GetAll().Where(t => t.TopicId == id).FirstOrDefault().TopicName;
        }
    }
}
