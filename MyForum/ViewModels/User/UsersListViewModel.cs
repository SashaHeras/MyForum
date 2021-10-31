using System.Linq;
using MyForum.Data.Models;

namespace MyForum.ViewModels
{
    public class UsersListViewModel
    {
        public IQueryable<User> AllUsers { get; set; }
    }
}
