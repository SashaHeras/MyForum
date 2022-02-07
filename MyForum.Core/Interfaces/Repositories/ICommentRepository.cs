using System.Linq;
using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Data.Models;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface ICommentRepository : ITransientService, IRepository<Comment>
    {
        public Comment GetCommentById(int id);

        public IQueryable<Comment> GetCommentsByPostId(int id);
    }
}
