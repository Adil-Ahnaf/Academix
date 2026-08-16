using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool IsPersistentCookie { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
