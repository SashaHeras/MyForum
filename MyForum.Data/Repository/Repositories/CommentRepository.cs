using System.Linq;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Models;

namespace MyForum.Data.Repository.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public Comment GetCommentById(int id)
        {
            return GetAll().Where(c => c.CommentId == id).FirstOrDefault();
        }

        public IQueryable<Comment> GetCommentsByPostId(int id)
        {
            return GetAll().Where(c => c.PostId == id);
        }
    }
}
