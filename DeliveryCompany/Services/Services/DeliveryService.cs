using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.Models;
using DeliveryCompany.Models.ViewModels;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Utility.Enums;
using Repository.IRepository;
using Serilog;
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

        public async Task<List<DeliveryOrdersVM>> GetOrdersByCarId(int carId, bool isAdmin)
        {
            List<DeliveryOrdersVM> deliveryOrdersVMs = new List<DeliveryOrdersVM>();
            var delivery = new List<DeliveryCarOrder>();
            if (!isAdmin)
            {
                delivery = _repositoryWrapper.DeliveryRepository.FindByCondition(d => d.DeliveryCarId.Equals(carId)).ToList().GroupBy(d => d.DeliveryId)
                    .Select(group => group.OrderBy(o => o.DateTime).First())
                    .ToList();
                Log.Information("Returning deliveries {@0} for Employee Car {1}...", delivery, carId);
            } else
            {
                delivery = _repositoryWrapper.DeliveryRepository.FindAll().ToList().GroupBy(d => d.DeliveryId)
                   .Select(group => group.OrderBy(o => o.DateTime).First())
                   .ToList();
                Log.Information("Returning deliveries {@0} for Admin...", delivery);
            }
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
            Log.Information("Returning deliveries...{@0}...", deliveryOrdersVMs);
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
            Log.Information("Retrieving all the deliveries for Id {1}...", deliveriesId);
            var delivieries = _repositoryWrapper.DeliveryRepository.FindByCondition(d => d.DeliveryId.Equals(deliveriesId)).ToList();
            Log.Information("{@0}",delivieries);
            foreach (var delivery in delivieries)
            {
                delivery.DeliveryStatus = DeliveryStatusEnum.InTransit;
                Log.Information("Retrieving order with Id {1}...", delivery.OrderId);
                var order = _repositoryWrapper.OrderRepository.FindByCondition(o => o.OrderId.Equals(delivery.OrderId)).FirstOrDefault();
                Log.Information("{@0}", order);
                order.OrderStatus = OrderStatus.InTransit;
                Log.Information("Updating Order {@0} and Delivery {@1}...",order, delivery);
                _repositoryWrapper.OrderRepository.Update(order);
                _repositoryWrapper.DeliveryRepository.Update(delivery);
            }
            var deliveryCar = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(dc => dc.DeliveryCarsId.Equals(delivieries.FirstOrDefault().DeliveryCarId)).FirstOrDefault();
            deliveryCar.DeliveryCarStatus = DeliveryCarStatus.Delivering;
            _repositoryWrapper.DeliveryCarsRepository.Update(deliveryCar);
            Log.Information("Updated delivert car {0} status to delivering...", deliveryCar.DeliveryCarsId);
            _repositoryWrapper.Save();
        }

        private async Task UpdateFinishedDeliveriesAsync(int deliveriesId)
        {
            Log.Information("Retrieving all the deliveries for Id {1}...", deliveriesId);
            var delivieries = _repositoryWrapper.DeliveryRepository.FindByCondition(d => d.DeliveryId.Equals(deliveriesId)).ToList();
            Log.Information("Finish existing deliveries {@0}...", delivieries);
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
            Log.Information("Updating delivery car {0} status to free...", deliveryCarId);
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
            Log.Information("Updating order {0} status to processing...", orderId);
            order.OrderStatus = OrderStatus.Processing;
            _repositoryWrapper.OrderRepository.Update(order);
            Log.Information("Creating or updating delivery for the order {0}...", orderId);
            await CreateOrUpdateDeliveryWithOrder(order);
            _repositoryWrapper.Save();
        }

        public async Task<bool> CreateOrUpdateDeliveryWithOrder(Order order)
        {
            Log.Information("Retrieving delivery cars that can deliver this order {@0}...", order);
            var deliveryCars = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(dc => (dc.CityId.Equals(order.CityId) && dc.DeliveryCarStatus.Equals(DeliveryCarStatus.Free) && dc.AssigmentStatus.Equals(AssigmentStatus.Assigned))).ToList();
            Log.Information("Retrieving package {0}...", order.PackagesId);
            var package = await _ordersService.GetPackagesByPackageIdIdAsync(order.PackagesId);
            bool isOrderCreated = false;
            Dictionary<int, int> deliveryIdToNumberOfOrders = new Dictionary<int, int>();
            foreach (var deliveryCar in deliveryCars)
            {
                var deliveryOrders = _repositoryWrapper.DeliveryRepository.FindByCondition(dco => dco.DeliveryCarId.Equals(deliveryCar.DeliveryCarsId) && dco.DeliveryStatus != DeliveryStatusEnum.InTransit && dco.DeliveryStatus != DeliveryStatusEnum.Finished).ToList();
                Log.Information("Cheking if the delivery car {0} has already other orders atached...", deliveryCar.DeliveryCarsId);
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
                        Log.Information("Creating new delivery for order {@0} and delivery car {@1}...", order, deliveryCar);
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
                    Log.Information("Sorting the deliveries so we can balance them {@1}...", sortedList);
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
                            Log.Information("Creating new delivery for order {@0} and delivery car {@1}...", order, deliveryCar);
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
            Log.Information("Creating a delivery for an already existing one {@0}...", deliveryOrder);
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
            Log.Information("Creating a new delivery {@0}...", delivery);
            _repositoryWrapper.DeliveryRepository.Create(delivery);
            _repositoryWrapper.Save();
        }
    }
}
