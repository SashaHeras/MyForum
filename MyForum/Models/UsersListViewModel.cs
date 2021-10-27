using MyForum.Controllers.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.ViewModels
{
    public class UsersListViewModel
    {
        public IQueryable<User> AllUsers { get; set; }
    }
}
