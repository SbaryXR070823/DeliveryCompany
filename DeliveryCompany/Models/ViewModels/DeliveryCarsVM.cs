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
	public class DeliveryCarsVM
	{
		public List<DeliveryCars> Cars { get; set; }
	}
}

