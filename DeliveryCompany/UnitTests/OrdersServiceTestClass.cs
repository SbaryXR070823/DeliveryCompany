using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Utility.Enums;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository.IRepository;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class OrdersServiceTestClass
    {
        private OrdersService _ordersService;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<ICityService> _mockCityService;

        [TestInitialize]
        public void Setup()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockCityService = new Mock<ICityService>();
            _ordersService = new OrdersService(_mockRepositoryWrapper.Object, _mockCityService.Object);
        }

        [TestMethod]
        public async Task GetOrdersAsync_WithoutUserId_ReturnsAllOrders()
        {
            var ordersList = new List<Order>
        {
            new Order { OrderId = 1, Price = 10.0, OrderStatus = OrderStatus.Processing, DateTime = DateTime.Now, UserId = "user1", PackagesId = 1, Address = "Address 1", CityId = 1 },
            new Order { OrderId = 2, Price = 15.0, OrderStatus = OrderStatus.Delivered, DateTime = DateTime.Now, UserId = "user2", PackagesId = 2, Address = "Address 2", CityId = 2 }
        };
            _mockRepositoryWrapper.Setup(x => x.OrderRepository.FindAll()).Returns(ordersList.AsQueryable());

            var packagesList = new List<Packages>
        {
            new Packages { PackagesId = 1, Name = "Package 1", Description = "Description 1", Weight = 5, Width = 10, Length = 15, Height = 20 },
            new Packages { PackagesId = 2, Name = "Package 2", Description = "Description 2", Weight = 10, Width = 15, Length = 20, Height = 25 }
        };
            _mockRepositoryWrapper.Setup(x => x.PackageRepository.FindByCondition(It.IsAny<Expression<Func<Packages, bool>>>()))
                      .Returns<Expression<Func<Packages, bool>>>(predicate => packagesList.Where(predicate.Compile()).AsQueryable());

            var result = await _ordersService.GetOrdersAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(ordersList.Count, result.Count);
            Assert.IsTrue(result.All(order => ordersList.Any(o => o.OrderId == order.Id)));
        }

        [TestMethod]
        public async Task GetOrdersAsync_WithUserId_ReturnsOrdersForUserId()
        {
            string userId = "user1";
            var ordersList = new List<Order>
        {
            new Order { OrderId = 1, Price = 10.0, OrderStatus = OrderStatus.Processing, DateTime = DateTime.Now, UserId = "user1", PackagesId = 1, Address = "Address 1", CityId = 1 },
            new Order { OrderId = 3, Price = 20.0, OrderStatus = OrderStatus.Processing, DateTime = DateTime.Now, UserId = "user1", PackagesId = 3, Address = "Address 3", CityId = 2 }
        };
            _mockRepositoryWrapper.Setup(x => x.OrderRepository.FindByCondition(It.IsAny<Expression<Func<Order, bool>>>()))
                      .Returns<Expression<Func<Order, bool>>>(predicate => ordersList.Where(predicate.Compile()).AsQueryable());
            var packagesList = new List<Packages>
        {
            new Packages { PackagesId = 1, Name = "Package 1", Description = "Description 1", Weight = 5, Width = 10, Length = 15, Height = 20 },
            new Packages { PackagesId = 3, Name = "Package 3", Description = "Description 3", Weight = 15, Width = 20, Length = 25, Height = 30 }
        };
            _mockRepositoryWrapper.Setup(x => x.PackageRepository.FindByCondition(It.IsAny<Expression<Func<Packages, bool>>>()))
                       .Returns<Expression<Func<Packages, bool>>>(predicate => packagesList.Where(predicate.Compile()).AsQueryable());

            var result = await _ordersService.GetOrdersAsync(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(order => order.CityId == 0));
        }
    }
}
