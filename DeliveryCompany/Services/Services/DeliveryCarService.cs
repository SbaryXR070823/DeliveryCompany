using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Services.IServices;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Services.Services
{
	public class DeliveryCarService : IDeliveryCarsService
	{
		private IRepositoryWrapper _repositoryWrapper;
		public DeliveryCarService(IRepositoryWrapper repositoryWrapper)
		{
			_repositoryWrapper = repositoryWrapper;
		}

		public List<DeliveryCars> GetAllCarsAsync()
		{
			var deliveryCars = _repositoryWrapper.DeliveryCarsRepository.FindAll().ToList();
			return deliveryCars;
		}

		public async Task DeleteDeliveryCarById(int deliveryCarId)
		{
			var deliverCar = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(dc => dc.DeliveryCarsId.Equals(deliveryCarId)).FirstOrDefault();
			_repositoryWrapper.DeliveryCarsRepository.Delete(deliverCar);
			_repositoryWrapper.Save();
		}
	}
}
