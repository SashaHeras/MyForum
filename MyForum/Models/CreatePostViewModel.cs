using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.ViewModels
{
    public class CreatePostViewModel
    {
        public String PostName { get; set; }
        public String Description { get; set; }
        public Int32 UserId { get; set; }
        public Int32 TopicId { get; set; }
        public Int32 PostId { get; set; }
    }
}
