using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Models.DbModels
{
    public class Packages
    {
        public int PackagesId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }

        public Order Order { get; set; }
    }
}
