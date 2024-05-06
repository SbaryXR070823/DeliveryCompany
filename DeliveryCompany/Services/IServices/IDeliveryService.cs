using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.ViewModels;
using DeliveryCompany.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Services.IServices
{
	public interface IDeliveryService
	{
		Task<List<DeliveryOrdersVM>> GetOrdersByCarId(int carId);
		Task<bool> CreateOrUpdateDeliveryWithOrder(Order order);
        Task UpdateDeliveries(int deliveriesId, DeliveryStatusEnum deliveryStatus);
    }
}
