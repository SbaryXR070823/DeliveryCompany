using DeliveryCompany.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Models.ViewModels
{
    public class UserCreationVM
    {
        [Required(ErrorMessage = "Name is required!")]
        public string? Name { get; set; } 

        [Required(ErrorMessage = "Email is required!")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; } 

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirmation password is required!")]
        [Compare("Password", ErrorMessage = "Passwords don't match!")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        [DataType(DataType.MultilineText)]
        public string? Address { get; set; } 

        [Required(ErrorMessage = "Role is required!")]
        public string? Role { get; set; } 

    }
}
