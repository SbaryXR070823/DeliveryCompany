using DeliveryCompany.Utility.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Models.DbModels
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public AssigmentStatus AssigmentStatus { get; set; }

        public DeliveryCars DeliveryCars { get; set;}
    }
}
