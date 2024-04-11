using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Utility.Enums;
using Microsoft.EntityFrameworkCore;
using Models.ViewModels;
using Repository.IRepository;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Helpers;

namespace Services.Services
{
    public class OrdersService : IOrdersService
    {
		private IRepositoryWrapper _repositoryWrapper;
		public OrdersService(IRepositoryWrapper repositoryWrapper)
		{
			_repositoryWrapper = repositoryWrapper;
		}

		public async Task<List<OrderVM>> GetOrdersAsync(string userId)
        {
			List<OrderVM> orderViewModels = new List<OrderVM>();

			var ordersList = _repositoryWrapper.OrderRepository.FindByCondition(x => x.UserId.Equals(userId)).ToList();
			foreach (var order in ordersList)
			{
				var package = _repositoryWrapper.PackageRepository.FindByCondition(x => x.PackagesId.Equals(order.PackagesId)).FirstOrDefault();
				OrderVM orderViewModel = new OrderVM
				{
					Id = order.OrderId,
					Price = order.Price,
					Name = package.Name,
					Description = package.Description,
					Weight = package.Weight,
					Width = package.Width,
					Length = package.Length,
					Height = package.Height,
					DateTime = order.DateTime,
					Status = order.OrderStatus
				};
				orderViewModels.Add(orderViewModel);
			}

			return orderViewModels;
		}

		public async Task CreateOrderAsync(Packages package, string userId, string address)
		{
			Packages packageOrder = new Packages
			{
				Width = package.Width,
				Height = package.Height,
				Name = package.Name,
				Description = package.Description,
				Weight = package.Weight,
				Length = package.Length
			};

			_repositoryWrapper.PackageRepository.Create(package);
			_repositoryWrapper.Save();

			Order order = new Order
			{
				OrderStatus = OrderStatus.Unassigned,
				DateTime = DateTime.Now,
				Price = OrderHelpers.CalculatePrice(package.Weight, package.Width, package.Length, package.Height),
				UserId = userId,
				Packages = package,
				Address = address
			};

			_repositoryWrapper.OrderRepository.Create(order);
			_repositoryWrapper.Save();
		}

        public async Task<bool> DeleteAsync(int id)
        {
			var order = _repositoryWrapper.OrderRepository.FindByCondition(x => x.OrderId.Equals(id)).FirstOrDefault();
			if (order is null)
			{
				return false;
			}
			var package = _repositoryWrapper.PackageRepository.FindByCondition(x => x.PackagesId.Equals(order.PackagesId)).FirstOrDefault();
			_repositoryWrapper.OrderRepository.Delete(order);
			_repositoryWrapper.Save();
			_repositoryWrapper.PackageRepository.Delete(package);
			_repositoryWrapper.Save();
			return true;
		}
    }
}
