﻿using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "First Name is Required")]
        public string FName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        public string LName { get; set; }
        [Required(ErrorMessage = "Email is Required"), EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required"), MinLength(5, ErrorMessage = "Minimum Password Length is 5"), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required"), Compare(nameof(Password), ErrorMessage = "Confirm Password does not match Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool IsAgree { get; set; }
    }
}
