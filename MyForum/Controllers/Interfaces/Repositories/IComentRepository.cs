using MyForum.Controllers.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Interfaces.Repositories
{
    public interface IComentRepository : IRepository<Coment>
    {
        public Coment GetComentById(Int32 id);

        public IQueryable<Coment> GetComentsByPostId(Int32 id);
    }
}
