using System.Linq;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Models;

namespace MyForum.Data.Repository.Repositories
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public TopicRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public Topic GetTopicById(int id)
        {
            return GetAll().FirstOrDefault(t => t.TopicId == id);
        }

        public IQueryable<Topic> GetTopicByName(string name)
        {
            return GetAll().Where(t => t.TopicName.Contains(name));
        }

        public string GetTopicNameById(int id)
        {
            return GetAll().FirstOrDefault(t => t.TopicId == id)?.TopicName;
        }
    }
}
