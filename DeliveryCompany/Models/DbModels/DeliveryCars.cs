using DeliveryCompany.Utility.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Models.DbModels
{
    public class DeliveryCars
    {
        [Key]
        public int DeliveryCarsId { get; set; }
        
        public int MaxWeight { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
        public int MaxLength { get; set; }
        public DeliveryCarStatus DeliveryCarStatus { get; set;}
        public AssigmentStatus AssigmentStatus { get; set;}

        public int CityId { get; set; }
        public int EmployeeId { get; set; }
        public City City { get; set; }
        public Employee Employee { get; set; }
    }
}
