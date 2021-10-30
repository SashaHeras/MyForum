using System.Linq;
using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Data.Models;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface IComentRepository :ITransientService, IRepository<Coment>
    {
        public Coment GetComentById(int id);

        public IQueryable<Coment> GetComentsByPostId(int id);
    }
}
