using MyForum.Controllers.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Dto.Post
{
    public class FullPost
    {
        public Int32 PostId { get; set; }
        public String PostName { get; set; }
        public String Description { get; set; }
        public Int32 PostUserId { get; set; }
        public Int32 PostTopicId { get; set; }
        public String PostUserName { get; set; }

        public FullPost()
        {

        }
    }
}
