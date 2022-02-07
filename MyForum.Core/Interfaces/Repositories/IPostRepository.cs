using System.Linq;
using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Data.Models;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface IPostRepository :ITransientService, IRepository<Post>
    {
        public Post GetPostById(int id);

        public Post GetPostByTopicId(int id);

        public IQueryable<Post> GetPostsByTopicId(int id);

        public IQueryable<Post> GetPostsByUserId(int id);
    }
}
