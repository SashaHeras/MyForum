using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Data.Repository.Repositories
{
    public class AdminRepository : Repository<User>, IAdminRepository
    {
        public AdminRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public User GetAdminById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);    
        }
    }
}
