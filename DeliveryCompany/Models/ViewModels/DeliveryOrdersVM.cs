using DeliveryCompany.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Models.ViewModels
{
	public class DeliveryOrdersVM
	{
		public DeliveryCarOrder DeliveryCarOrder { get; set; }
		public List<Order> OrderList { get; set; } = new List<Order>();
	}
}
