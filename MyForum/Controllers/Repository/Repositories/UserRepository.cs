using MyForum.Controllers.Data;
using MyForum.Controllers.Data.Models;
using MyForum.Controllers.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public User GetUserById(int id)
        {
            return GetAll().Where(u => u.Id == id).FirstOrDefault();
        }

        public String GetUserName(User u)
        {
            return u.Name;
        }

        public String GetUserNameById(int id)
        {
            return GetAll().Where(u => u.Id == id).FirstOrDefault().Name;
        }

        public User GetUserNameByEmail(String email)
        {
            return GetAll().Where(u => u.Email.CompareTo(email) == 0).FirstOrDefault();
        }
    }
}
