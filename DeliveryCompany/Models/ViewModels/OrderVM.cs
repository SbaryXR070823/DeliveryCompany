using DeliveryCompany.Utility.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class OrderVM
    {
        [Key]
        public int Id { get; set; }
        public double Price {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public DateTime DateTime { get; set; }
        public OrderStatus Status { get; set; }
    }
}
