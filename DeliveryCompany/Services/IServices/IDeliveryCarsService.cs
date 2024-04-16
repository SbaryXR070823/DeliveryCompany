using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Services.IServices
{
	public interface IDeliveryCarsService
	{
		List<DeliveryCars> GetAllCarsAsync();
		Task DeleteDeliveryCarById(int deliveryCarId);
        Task AddNewDeliveryCar(DeliveryCarCreationVM deliveryCarCreationVM);
        Task<DeliveryCars> GetDeliveryCarById(int deliveryCarId);
        Task UpdateDeliveryCar(DeliveryCarCreationVM deliveryCarVM);
    }
}
