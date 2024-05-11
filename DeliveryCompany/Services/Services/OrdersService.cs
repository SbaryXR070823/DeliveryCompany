using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.Models;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Utility.Enums;
using Microsoft.EntityFrameworkCore;
using Models.ViewModels;
using Repository.IRepository;
using Serilog;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Helpers;

namespace Services.Services
{
    public class OrdersService : IOrdersService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private readonly ICityService _cityService;
        public OrdersService(IRepositoryWrapper repositoryWrapper, ICityService cityService)
        {
            _repositoryWrapper = repositoryWrapper;
            _cityService = cityService;
        }


        public async Task<List<OrderVM>> GetOrdersAsync(string userId = null)
        {
            List<OrderVM> orderViewModels = new List<OrderVM>();
            List<Order>? ordersList = new List<Order>();
            if (string.IsNullOrEmpty(userId))
            {
                Log.Information("Retrieving all the orders...");
                ordersList = _repositoryWrapper.OrderRepository.FindAll().ToList();
            }
            else
            {
                Log.Information("Retrieving orders for userId {0}", userId);
                ordersList = _repositoryWrapper.OrderRepository.FindByCondition(x => x.UserId.Equals(userId)).ToList();
            }
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

        public async Task<Order> CreateOrderAsync(Packages package, UserOrderInformations userOrderInformations)
        {
            _repositoryWrapper.PackageRepository.Create(package);
            _repositoryWrapper.Save();

            Order order = new Order
            {
                OrderStatus = OrderStatus.Unassigned,
                DateTime = DateTime.Now,
                Price = OrderHelpers.CalculatePrice(package.Weight, package.Width, package.Length, package.Height),
                UserId = userOrderInformations.UserId,
                Packages = package,
                Address = userOrderInformations.UserAddress,
                CityId = userOrderInformations.UserCityId
            };
            Log.Information("Creating new order with the content {@0}", package);
            _repositoryWrapper.OrderRepository.Create(order);
            _repositoryWrapper.Save();
            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = _repositoryWrapper.OrderRepository.FindByCondition(x => x.OrderId.Equals(id)).FirstOrDefault();
            if (order is null)
            {
                return false;
            }
            var package = _repositoryWrapper.PackageRepository.FindByCondition(x => x.PackagesId.Equals(order.PackagesId)).FirstOrDefault();
            Log.Information("Deleting order {@0} and package {@1}...", order, package);
            _repositoryWrapper.OrderRepository.Delete(order);
            _repositoryWrapper.Save();
            _repositoryWrapper.PackageRepository.Delete(package);
            _repositoryWrapper.Save();
            var delivery = _repositoryWrapper.DeliveryRepository.FindByCondition(d => d.OrderId.Equals(id)).FirstOrDefault();
            if (delivery is not null)
            {
                Log.Information("Deleting delivery {@0}...", delivery);
                _repositoryWrapper.DeliveryRepository.Delete(delivery);
            }
            _repositoryWrapper.Save();
            return true;
        }

        public async Task<OrderVM> GetCitiesWithOrderViewModel()
        {
            Log.Information("Retrieving all the cities...");
            var cities = await _cityService.GetCitiesAsync();
            OrderVM orderViewModel = new OrderVM();
            orderViewModel.Cities = cities;
            return orderViewModel;
        }

        public async Task<Packages> GetPackagesByPackageIdIdAsync(int packageId)
        {
            Log.Information("Retrieving the package by packageId {0}...", packageId);
            var package = _repositoryWrapper.PackageRepository.FindByCondition(p => p.PackagesId.Equals(packageId)).FirstOrDefault();
            return package;
        }

        public async Task UpdateStatusOfOrder(int orderId, OrderStatus orderStatus)
        {
            var order = _repositoryWrapper.OrderRepository.FindByCondition(o => o.OrderId.Equals(orderId)).FirstOrDefault();
            order.OrderStatus = orderStatus;
            Log.Information("Updating order {0} status to {1}...",orderId, orderStatus);
            _repositoryWrapper.OrderRepository.Update(order);
            _repositoryWrapper.Save();
        }
    }
}
