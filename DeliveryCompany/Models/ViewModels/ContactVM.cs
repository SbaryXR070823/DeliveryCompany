using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Models.ViewModels
{
    public class ContactVM
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required!")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Message is required!")]
        [MinLength(50, ErrorMessage = "Message should have more than 50 characters")]
        [MaxLength(500, ErrorMessage = "Message should have less than 500 characters")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
        public string? UserId { get; set; }
    }
}
