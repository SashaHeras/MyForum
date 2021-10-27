using MyForum.Controllers.Data;
using MyForum.Controllers.Data.Models;
using MyForum.Controllers.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Repository.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public Post GetPostById(int id)
        {
            return GetAll().Where(p => p.PostId == id).FirstOrDefault();
        }

        public Post GetPostByTopicId(int id)
        {
            return GetAll().Where(p => p.TopicId == id).FirstOrDefault();
        }

        public IQueryable<Post> GetPostsByTopicId(int id)
        {
            return GetAll().Where(p => p.TopicId == id);
        }

        public IQueryable<Post> GetPostsByUserId(int id)
        {
            return GetAll().Where(p => p.UserId == id);
        }
    }
}
