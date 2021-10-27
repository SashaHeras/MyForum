using MyForum.Controllers.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Interfaces.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        public Post GetPostById(int id);

        public Post GetPostByTopicId(int id);

        public IQueryable<Post> GetPostsByTopicId(int id);

        public IQueryable<Post> GetPostsByUserId(int id);
    }
}
