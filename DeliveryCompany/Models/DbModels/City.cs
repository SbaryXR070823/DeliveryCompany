using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Models.DbModels
{
    public class City
    {
        [Key]
        [Column(Order = 1)]
        public int CityId { get; set; }
        public string Name { get; set; }

        public ICollection<DeliveryCars> DeliveryCarsInCity { get;} = new List<DeliveryCars>();
     
    }
}
