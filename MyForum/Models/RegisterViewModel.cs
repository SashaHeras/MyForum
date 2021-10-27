using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public String Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Field {0} must have more than {2} and less than {1} symbols!",MinimumLength = 8)]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [Required]
        [Compare("Password",ErrorMessage ="Password is not correct!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confim password")]
        public String PasswordConfim { get; set; }
    }
}
