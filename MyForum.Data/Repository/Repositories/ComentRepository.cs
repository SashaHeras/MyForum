using System.Linq;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Models;

namespace MyForum.Data.Repository.Repositories
{
    public class ComentRepository : Repository<Coment>, IComentRepository
    {
        public ComentRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public Coment GetComentById(int id)
        {
            return GetAll().Where(c => c.ComentId == id).FirstOrDefault();
        }

        public IQueryable<Coment> GetComentsByPostId(int id)
        {
            return GetAll().Where(c => c.PostId == id);
        }
    }
}
