using MyForum.Core.Interfaces.Infrastructure;
using MyForum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface IComplaintRepository: ITransientService, IRepository<Complaint>
    {
        public Complaint GetComplaintById(int id);
    }
}
