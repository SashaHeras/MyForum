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
    public class MarkRepository : Repository<UserPostMark>, IMarkRepository
    {
        public MarkRepository(MyForumContext forumContext) : base(forumContext)
        {

        }

        public UserPostMark GetMarkById(int id)
        {
            return GetAll().Where(m => m.Id == id).FirstOrDefault();
        }

        public IQueryable<UserPostMark> GetMarksByPostId(int id)
        {
            return GetAll().Where(m => m.PostId == id);
        }

        public UserPostMark GetMarkByUserAndPostId(int ui, int pi)
        {
            return GetAll().Where(m => m.UserId == ui && m.PostId == pi).FirstOrDefault();
        }

        public int GetPostMark(int id)
        {
            return GetAll().Where(p => p.PostId == id).Sum(m => m.PostMark);
        }

        public UserPostMark GetUsersMarkById(int id)
        {
            return GetAll().Where(m => m.UserId == id).FirstOrDefault();
        }
    }
}
