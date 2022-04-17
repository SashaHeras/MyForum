using System.Collections.Generic;
using System.Linq;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Models;

namespace MyForum.Data.Repository.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public IQueryable<Post> GetAllowedPostsByTopicId(int id)
        {
            return GetAll().Where(p => p.TopicId == id && p.IsAllow == true);
        }

        public IEnumerable<Post> GetPopularAllowedPosts()
        {
            return GetAll().OrderByDescending(p => p.Views).Take(5);
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
