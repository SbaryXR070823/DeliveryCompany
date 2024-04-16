using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Utility.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Models.ViewModels
{
    public class DeliveryCarCreationVM
    {
        [Required(ErrorMessage = "MaxWeight is required!")]
        [Range(800, 2000, ErrorMessage = "Weight must be between 800 and 2000 kg.")]
        public int MaxWeight { get; set; }
        [Required(ErrorMessage = "MaxWidth is required!")]
        [Range(120, 250, ErrorMessage = "Width must be between 120 and 250 cm.")]
        public int MaxWidth { get; set; }
        [Required(ErrorMessage = "MaxHeight is required!")]
        [Range(120, 200, ErrorMessage = "Height must be between 120 and 200 cm.")]
        public int MaxHeight { get; set; }
        [Required(ErrorMessage = "MaxLength is required!")]
        [Range(180, 300, ErrorMessage = "Length must be between 180 and 300 cm.")]
        public int MaxLength { get; set; }

        public int? EmployeeId { get; set; }

        public int? DeliveryCarsId { get; set; }
 
        public List<Employee>? Employees {  get; set; }

		[Required(ErrorMessage = "CityId is required!")]
		public int CityId { get; set; }

    }
}
