using MyForum.Controllers.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public User GetUserById(int id);

        public String GetUserName(User u);
    }
}
