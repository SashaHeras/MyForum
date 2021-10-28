using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Data.Models
{
    public class Coment
    {
        public Int32 ComentId { get; set; }
        public String Comment { get; set; }
        public Int32 PostId { get; set; }
        public Int32 UserId { get; set; }
    }
}
