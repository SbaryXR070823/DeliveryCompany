using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DeliveryCompany.Utility.Enums;

namespace DeliveryCompany.Models.DbModels
{
    public class DeliveryCarOrder
    {
        [ForeignKey("DeliveryCarId")]
        [Column(Order = 2)]
        public int DeliveryCarId { get; set; }

        [ForeignKey("OrderId")]
        [Column(Order = 3)]
        public int OrderId { get; set; }

        public DeliveryStatusEnum DeliveryStatus { get; set; }

        public DateTime DateTime { get; set; }

        [Column(Order = 1)]
        public int DeliveryId { get; set; }

        public DeliveryCars DeliveryCars { get; set; }
        public Order Order { get; set; }
    }
}
