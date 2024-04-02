using DeliveryCompany.Utility.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Models.DbModels
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public double Price { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime DateTime { get; set; }
        public string UserId { get; set; }
        public int PackagesId { get; set; }

        public Packages Packages { get; set; }
    }
}
