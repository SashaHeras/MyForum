using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface IAdminRepository : ISingletonService, IRepository<User>
    {
        public User GetAdminById(int id);
    }
}
