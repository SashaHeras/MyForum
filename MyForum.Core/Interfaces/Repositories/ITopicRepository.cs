using System.Linq;
using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Data.Models;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface ITopicRepository : ITransientService, IRepository<Topic>
    {
        public Topic GetTopicById(int id);

        public IQueryable<Topic> GetTopicByName(string name);

        public IQueryable<Topic> GetAllowedTopics();

        public IQueryable<Topic> SearchAllowedTopics(string topic);
    }
}
