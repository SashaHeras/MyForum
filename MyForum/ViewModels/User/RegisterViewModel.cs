using System.ComponentModel.DataAnnotations;

namespace MyForum.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Field {0} must have more than {2} and less than {1} symbols!",MinimumLength = 8)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password",ErrorMessage ="Password is not correct!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confim password")]
        public string PasswordConfim { get; set; }
    }
}
