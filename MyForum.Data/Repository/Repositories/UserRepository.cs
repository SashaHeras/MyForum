using System.Linq;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Models;

namespace MyForum.Data.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public User GetUserById(int id)
        {
            return GetAll().FirstOrDefault(u => u.Id == id);
        }

        public string GetUserName(User u)
        {
            return u.Name;
        }

        public string GetUserNameById(int id)
        {
            return GetAll().Where(u => u.Id == id).FirstOrDefault().Name;
        }

        public User GetUserNameByEmail(string email)
        {
            return GetAll().Where(u => u.Email.CompareTo(email) == 0).FirstOrDefault();
        }
    }
}
