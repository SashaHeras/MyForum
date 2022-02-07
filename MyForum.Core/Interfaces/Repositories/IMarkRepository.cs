using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface IMarkRepository : ITransientService, IRepository<UserPostMark>
    {
        public UserPostMark GetMarkById(int id);

        public UserPostMark GetUsersMarkById(int id);

        public IQueryable<UserPostMark> GetMarksByPostId(int id);
    }
}
