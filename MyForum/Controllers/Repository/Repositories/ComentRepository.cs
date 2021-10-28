using MyForum.Controllers.Data;
using MyForum.Controllers.Data.Models;
using MyForum.Controllers.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Repository.Repositories
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
