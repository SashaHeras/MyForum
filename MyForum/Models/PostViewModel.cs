using MyForum.Controllers.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.ViewModels
{
    public class PostViewModel
    {
        public Post GetPostByTopicId { get; set; }
        public String GetTopicNameById { get; set; }
    }
}
