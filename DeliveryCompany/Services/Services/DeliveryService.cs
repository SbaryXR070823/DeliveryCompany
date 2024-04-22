﻿using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.Models;
using DeliveryCompany.Models.ViewModels;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Utility.Enums;
using Repository.IRepository;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Services.Services
{
    public class DeliveryService : IDeliveryService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private readonly ICityService _cityService;
        private readonly IOrdersService _ordersService;
        public DeliveryService(IRepositoryWrapper repositoryWrapper, ICityService cityService, IOrdersService ordersService)
        {
            _repositoryWrapper = repositoryWrapper;
            _cityService = cityService;
            _ordersService = ordersService;
        }

        public async Task<List<DeliveryOrdersVM>> GetOrdersByEmployeeId(int carId)
        {
            List<DeliveryOrdersVM> deliveryOrdersVMs = new List<DeliveryOrdersVM>();
            var delivery = _repositoryWrapper.DeliveryRepository.FindByCondition(d => d.DeliveryCarId.Equals(carId)).ToList();
            foreach (var deliveryOrder in delivery)
            {
                var orders = _repositoryWrapper.OrderRepository.FindByCondition(o => o.OrderId.Equals(deliveryOrder.OrderId)).ToList();
                DeliveryOrdersVM deliveryOrderVMs = new DeliveryOrdersVM
                {
                    DeliveryCarOrder = deliveryOrder,
                    OrderList = orders
                };
                deliveryOrdersVMs.Add(deliveryOrderVMs);
            }
            return deliveryOrdersVMs;

        }

        public async Task CreateOrUpdateDeliveryWithOrder(Order order)
        {
            var deliveryCars = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(dc => (dc.CityId.Equals(order.CityId) && dc.DeliveryCarStatus.Equals(DeliveryCarStatus.Free) && dc.AssigmentStatus.Equals(AssigmentStatus.Assigned))).ToList();
            var package = await _ordersService.GetPackagesByPackageIdIdAsync(order.PackagesId);
            bool isOrderCreated = false;
            Dictionary<int, int> deliveryIdToNumberOfOrders = new Dictionary<int, int>();
            foreach (var deliveryCar in deliveryCars)
            {
                if (isOrderCreated)
                {
                    break;
                }
                var deliveryOrders = _repositoryWrapper.DeliveryRepository.FindByCondition(dco => dco.DeliveryCarId.Equals(deliveryCar.DeliveryCarsId)).ToList();
                if (deliveryOrders.Count == 0)
                {
                    if (deliveryCar.MaxHeight > package.Height &&
                        deliveryCar.MaxWeight > package.Weight &&
                        deliveryCar.MaxWidth > package.Width &&
                        deliveryCar.MaxLength > package.Length)
                    {
                        DeliveryCarOrder delivery = new DeliveryCarOrder()
                        {
                            DeliveryCarId = deliveryCar.DeliveryCarsId,
                            DateTime = DateTime.Now,
                            DeliveryStatus = DeliveryStatusEnum.Pending,
                            OrderId = order.OrderId,
                        };

                        _repositoryWrapper.DeliveryRepository.Create(delivery);
                        _repositoryWrapper.Save();
                        isOrderCreated = true;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (!deliveryIdToNumberOfOrders.ContainsKey(deliveryCar.DeliveryCarsId))
                    {
                        deliveryIdToNumberOfOrders[deliveryCar.DeliveryCarsId] = deliveryOrders.Count;

                    }
                }
            }
            if (!isOrderCreated)
            {
                if (deliveryIdToNumberOfOrders.Count > 0)
                {
                    List<KeyValuePair<int, int>> sortedList = deliveryIdToNumberOfOrders.ToList();
                    sortedList.Sort((x, y) => x.Value.CompareTo(y.Value));
                    foreach (var dcv in sortedList)
                    {
                        if (isOrderCreated)
                        {
                            break;
                        }
                        var deliveryCar = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(dc => dc.DeliveryCarsId.Equals(dcv.Key)).FirstOrDefault();
                        if (deliveryCar.MaxHeight > package.Height &&
                       deliveryCar.MaxWeight > package.Weight &&
                       deliveryCar.MaxWidth > package.Width &&
                       deliveryCar.MaxLength > package.Length)
                        {
                            DeliveryCarOrder delivery = new DeliveryCarOrder()
                            {
                                DeliveryCarId = deliveryCar.DeliveryCarsId,
                                DateTime = DateTime.Now,
                                DeliveryStatus = DeliveryStatusEnum.Pending,
                                OrderId = order.OrderId,
                            };

                            _repositoryWrapper.DeliveryRepository.Create(delivery);
                            _repositoryWrapper.Save();
                            isOrderCreated = true;
                        }
                    }
                }

            }

        }
    }
}
