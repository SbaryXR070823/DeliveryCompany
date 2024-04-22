﻿using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Services.IServices
{
	public interface IDeliveryService
	{
		Task<List<DeliveryOrdersVM>> GetOrdersByEmployeeId(int carId);
		Task CreateOrUpdateDeliveryWithOrder(Order order);

    }
}