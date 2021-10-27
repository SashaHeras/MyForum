using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Data.Models
{
    public class User
    {
        public Int32 Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public String Surname { get; set; }

        [Required]
        [Display(Name = "Age")]
        public Int32 Age { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Required]
        [Display(Name = "Address")]
        public String Address { get; set; }
    }
}
