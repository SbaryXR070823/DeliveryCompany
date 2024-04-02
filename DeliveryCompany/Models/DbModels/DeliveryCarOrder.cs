using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DeliveryCompany.Models.DbModels
{
    public class DeliveryCarOrder
    {
        [Key]
        [Column(Order = 1)]
        public int DeliveryCarId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int OrderId { get; set; }
        
        public DeliveryCars DeliveryCars { get; set; }
        public Order Order { get; set; }
    }
}
