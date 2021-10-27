using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public String Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [Display(Name = "Remember?")]
        public bool RememberMyself { get; set; }
    }
}
