using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Email is Required"), Display(Name = "UserName or Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required"), MinLength(5, ErrorMessage = "Minimum Password Length is 5"), DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
