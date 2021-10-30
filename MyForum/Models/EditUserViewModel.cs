using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Models
{
    public class EditUserViewModel
    {
        public String Name { get; set; }
        public String Surname { get; set; }
        public Int32 Age { get; set; }
        public String Email { get; set; }
        public String Address { get; set; }
    }
}
