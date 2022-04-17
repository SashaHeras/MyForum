using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Data.Dto.Post
{
    public class AdminPost
    {
        public int PostId { get; set; }

        public string PostName { get; set; }

        public int TopicId { get; set; }

        public string Description { get; set; }

        public string UserName { get; set; }

        public string Updated { get; set; }

        public int UserId { get; set; }

        public bool IsAllow { get; set; }
    }
}
