using MyForum.Core.Interfaces.Repositories;
using MyForum.Core.Models;
using MyForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Data.Repository.Repositories
{
    public class ComplaintRepository : Repository<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(MyForumContext repositoryContext) : base(repositoryContext)
        {

        }

        public Complaint GetComplaintById(int id)
        {
            return GetAll().Where(c=>c.Id == id).FirstOrDefault();
        }
    }
}
