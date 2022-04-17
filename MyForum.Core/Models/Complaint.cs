using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Core.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int PostId { get; set; }
    }
}
