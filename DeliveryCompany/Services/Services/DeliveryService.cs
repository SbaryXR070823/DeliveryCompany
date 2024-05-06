using DeliveryCompany.Models.DbModels;
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
using Utility.Helpers;
using Utility.Models;

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

        public async Task<List<DeliveryOrdersVM>> GetOrdersByCarId(int carId)
        {
            List<DeliveryOrdersVM> deliveryOrdersVMs = new List<DeliveryOrdersVM>();
            var delivery = _repositoryWrapper.DeliveryRepository.FindByCondition(d => d.DeliveryCarId.Equals(carId)).ToList().GroupBy(d => d.DeliveryId)
                .Select(group => group.OrderBy(o => o.DateTime).First())
                .ToList();
            foreach (var deliveryOrder in delivery)
            {
                var deliveries = _repositoryWrapper.DeliveryRepository.FindByCondition(d => d.DeliveryCarId.Equals(deliveryOrder.DeliveryCarId) && d.DeliveryId.Equals(deliveryOrder.DeliveryId)).ToList();
                var OrderList = new List<Order>();
                foreach (var deliveryToAdd in deliveries)
                {
                    var order = _repositoryWrapper.OrderRepository.FindByCondition(o => o.OrderId.Equals(deliveryToAdd.OrderId)).FirstOrDefault();
                    OrderList.Add(order);
                }
                DeliveryOrdersVM deliveryOrderVMs = new DeliveryOrdersVM
                {
                    DeliveryCarOrder = deliveryOrder,
                    OrderList = OrderList
                };
                deliveryOrdersVMs.Add(deliveryOrderVMs);
            }
            return deliveryOrdersVMs;
        }

        public async Task UpdateDeliveries(int deliveriesId, DeliveryStatusEnum deliveryStatus)
        {
            switch (deliveryStatus)
            {
                case DeliveryStatusEnum.InTransit:
                    UpdateInTransitDeliveries(deliveriesId);
                    break;
                case DeliveryStatusEnum.Finished:
                    await UpdateFinishedDeliveriesAsync(deliveriesId);
                    break;
            }
        }

        private void UpdateInTransitDeliveries(int deliveriesId)
        {
            var delivieries = _repositoryWrapper.DeliveryRepository.FindByCondition(d => d.DeliveryId.Equals(deliveriesId)).ToList();
            foreach (var delivery in delivieries)
            {
                delivery.DeliveryStatus = DeliveryStatusEnum.InTransit;
                var order = _repositoryWrapper.OrderRepository.FindByCondition(o => o.OrderId.Equals(delivery.OrderId)).FirstOrDefault();
                order.OrderStatus = OrderStatus.InTransit;
                _repositoryWrapper.OrderRepository.Update(order);
                _repositoryWrapper.DeliveryRepository.Update(delivery);
            }
            var deliveryCar = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(dc => dc.DeliveryCarsId.Equals(delivieries.FirstOrDefault().DeliveryCarId)).FirstOrDefault();
            deliveryCar.DeliveryCarStatus = DeliveryCarStatus.Delivering;
            _repositoryWrapper.Save();
        }

        private async Task UpdateFinishedDeliveriesAsync(int deliveriesId)
        {
            var delivieries = _repositoryWrapper.DeliveryRepository.FindByCondition(d => d.DeliveryId.Equals(deliveriesId)).ToList();
            FinishExistingDeliveries(delivieries);
            UpdateDeliveryCarStatus(delivieries.FirstOrDefault().DeliveryCarId, DeliveryCarStatus.Free);
            foreach (var delivery in delivieries)
            {
                var order = _repositoryWrapper.OrderRepository.FindByCondition(o => o.OrderId.Equals(delivery.OrderId)).FirstOrDefault();
                switch (order.OrderStatus)
                {
                    case OrderStatus.InTransit:
                        await UpdateFinishedInTransitDeliveriesAsync(delivery.OrderId);
                        break;
                    default:
                        break;
                }
            }
            await CheckForNewDeliveriesAsync(delivieries.FirstOrDefault().DeliveryCarId);
            _repositoryWrapper.Save();
        }

        private void FinishExistingDeliveries(List<DeliveryCarOrder?> deliveryCarOrders)
        {
            foreach (var delivery in deliveryCarOrders)
            {
                delivery.DeliveryStatus = DeliveryStatusEnum.Finished;
                _repositoryWrapper.DeliveryRepository.Update(delivery);
                _repositoryWrapper.Save();
            }
        }



        private void UpdateDeliveryCarStatus(int deliveryCarId, DeliveryCarStatus status)
        {
            var deliveryCar = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(dc => dc.DeliveryCarsId.Equals(deliveryCarId)).FirstOrDefault();
            deliveryCar.DeliveryCarStatus = status;
            _repositoryWrapper.DeliveryCarsRepository.Update(deliveryCar);
            _repositoryWrapper.Save();
        }

        private async Task CheckForNewDeliveriesAsync(int deliveryCarId)
        {
            var deliveryCar = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(dc => dc.DeliveryCarsId.Equals(deliveryCarId)).FirstOrDefault();
            var orders = _repositoryWrapper.OrderRepository.FindByCondition(o => o.CityId.Equals(deliveryCar.CityId) && o.OrderStatus.Equals(OrderStatus.Unassigned)).ToList();
            foreach (var order in orders)
            {
                await CreateOrUpdateDeliveryWithOrder(order);
            }
        }


        private async Task UpdateFinishedInTransitDeliveriesAsync(int orderId)
        {
            var order = _repositoryWrapper.OrderRepository.FindByCondition(o => o.OrderId.Equals(orderId)).FirstOrDefault();
            order.OrderStatus = OrderStatus.Processing;
            _repositoryWrapper.OrderRepository.Update(order);
            await CreateOrUpdateDeliveryWithOrder(order);
            _repositoryWrapper.Save();
        }

        public async Task<bool> CreateOrUpdateDeliveryWithOrder(Order order)
        {
            var deliveryCars = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(dc => (dc.CityId.Equals(order.CityId) && dc.DeliveryCarStatus.Equals(DeliveryCarStatus.Free) && dc.AssigmentStatus.Equals(AssigmentStatus.Assigned))).ToList();
            var package = await _ordersService.GetPackagesByPackageIdIdAsync(order.PackagesId);
            bool isOrderCreated = false;
            Dictionary<int, int> deliveryIdToNumberOfOrders = new Dictionary<int, int>();
            foreach (var deliveryCar in deliveryCars)
            {
                var deliveryOrders = _repositoryWrapper.DeliveryRepository.FindByCondition(dco => dco.DeliveryCarId.Equals(deliveryCar.DeliveryCarsId) && dco.DeliveryStatus != DeliveryStatusEnum.InTransit && dco.DeliveryStatus != DeliveryStatusEnum.Finished).ToList();
                if (deliveryOrders.Count == 0)
                {
                    var packageToCheck = new PackageCheck
                    {
                        MaxHeight = deliveryCar.MaxHeight,
                        MaxLength = deliveryCar.MaxLength,
                        MaxWeight = deliveryCar.MaxWeight,
                        MaxWidth = deliveryCar.MaxWidth,
                        Height = package.Height,
                        Width = package.Width,
                        Length = package.Length,
                        Weight = package.Weight,
                    };
                    if (OrderHelpers.CheckPackage(packageToCheck))
                    {
                        CreateNewDelivery(deliveryCar, order);
                        isOrderCreated = true;
                        return true;
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
                        var deliveryCar = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(dc => dc.DeliveryCarsId.Equals(dcv.Key)).FirstOrDefault();
                        var packageToCheck = new PackageCheck
                        {
                            MaxHeight = deliveryCar.MaxHeight,
                            MaxLength = deliveryCar.MaxLength,
                            MaxWeight = deliveryCar.MaxWeight,
                            MaxWidth = deliveryCar.MaxWidth,
                            Height = package.Height,
                            Width = package.Width,
                            Length = package.Length,
                            Weight = package.Weight,
                        };
                        if (OrderHelpers.CheckPackage(packageToCheck))
                        {
                            CreateDeliveryForAlreadyExistingOne(deliveryCar, order);
                            isOrderCreated = true;
                            return true;
                        }
                    }
                }

            }
            return false;
        }

        private void CreateDeliveryForAlreadyExistingOne(DeliveryCars deliveryCar, Order order)
        {
            var deliveryOrder = _repositoryWrapper.DeliveryRepository.FindByCondition(dco => dco.DeliveryCarId.Equals(deliveryCar.DeliveryCarsId) && dco.DeliveryStatus == DeliveryStatusEnum.Pending).FirstOrDefault();
            DeliveryCarOrder delivery = new DeliveryCarOrder()
            {
                DeliveryCarId = deliveryCar.DeliveryCarsId,
                DateTime = DateTime.Now,
                DeliveryStatus = DeliveryStatusEnum.Pending,
                OrderId = order.OrderId,
                DeliveryId = deliveryOrder.DeliveryId,
            };

            _repositoryWrapper.DeliveryRepository.Create(delivery);
            _repositoryWrapper.Save();
        }

        private void CreateNewDelivery(DeliveryCars deliveryCar, Order order)
        {
                var lastDelivery = _repositoryWrapper.DeliveryRepository.FindAll().OrderByDescending(x => x.DeliveryId).ToList();
                DeliveryCarOrder delivery = new DeliveryCarOrder()
                {
                    DeliveryCarId = deliveryCar.DeliveryCarsId,
                    DateTime = DateTime.Now,
                    DeliveryStatus = DeliveryStatusEnum.Pending,
                    OrderId = order.OrderId,
                    DeliveryId = lastDelivery.FirstOrDefault() is null ? 1 : (lastDelivery.FirstOrDefault().DeliveryId + 1),
                };

                _repositoryWrapper.DeliveryRepository.Create(delivery);
                _repositoryWrapper.Save();
        }
    }
}
