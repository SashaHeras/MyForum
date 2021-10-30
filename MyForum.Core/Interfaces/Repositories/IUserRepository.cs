using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Data.Models;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface IUserRepository : ITransientService, IRepository<User>
    {
        public User GetUserById(int id);

        public string GetUserName(User u);
    }
}
