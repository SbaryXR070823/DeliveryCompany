using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Services.Services;
using DeliveryCompany.Utility.Enums;
using Moq;
using Repository.IRepository;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utility.Models;

namespace UnitTests
{
    [TestClass]
    public class DeliveryServiceTestClass
    {
        [TestClass]
        public class DeliveryServiceTests
        {
            private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
            private Mock<ICityService> _mockCityService;
            private Mock<IOrdersService> _mockOrdersService;
            private DeliveryService _deliveryService;

            [TestInitialize]
            public void TestInitialize()
            {
                _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
                _mockCityService = new Mock<ICityService>();
                _mockOrdersService = new Mock<IOrdersService>();
                _deliveryService = new DeliveryService(_mockRepositoryWrapper.Object, _mockCityService.Object, _mockOrdersService.Object);
            }

            [TestMethod]
            public async Task CreateOrUpdateDeliveryWithOrder_NoDeliveries()
            {
                var order = new Order { CityId = 1, PackagesId = 1, OrderId = 1 };
                var package = new Packages { PackagesId = 1, Height = 1, Length = 1, Weight = 1, Width = 1 };
                var deliveryCars = new List<DeliveryCars>
            {
                new DeliveryCars { DeliveryCarsId = 1, CityId = 1, MaxHeight = 10, MaxLength = 10, MaxWeight = 10, MaxWidth = 10, DeliveryCarStatus = DeliveryCarStatus.Free, AssigmentStatus = AssigmentStatus.Assigned }
            };

                _mockOrdersService.Setup(x => x.GetPackagesByPackageIdIdAsync(order.PackagesId)).ReturnsAsync(package);
                _mockRepositoryWrapper.Setup(x => x.DeliveryCarsRepository.FindByCondition(It.IsAny<Expression<Func<DeliveryCars, bool>>>()))
                    .Returns(deliveryCars.AsQueryable());
                _mockRepositoryWrapper.Setup(x => x.DeliveryRepository.FindByCondition(It.IsAny<Expression<Func<DeliveryCarOrder, bool>>>()))
                    .Returns(Enumerable.Empty<DeliveryCarOrder>().AsQueryable());
                _mockRepositoryWrapper.Setup(x => x.DeliveryRepository.FindAll()).Returns(Enumerable.Empty<DeliveryCarOrder>().AsQueryable());

                var result = await _deliveryService.CreateOrUpdateDeliveryWithOrder(order);

                Assert.IsTrue(result);
                _mockRepositoryWrapper.Verify(x => x.DeliveryRepository.Create(It.IsAny<DeliveryCarOrder>()), Times.Once);
            }

            [TestMethod]
            public async Task CreateOrUpdateDeliveryWithOrder_TwoDeliveriesWithTwoOrdersEach()
            {
                var order = new Order { CityId = 1, PackagesId = 1, OrderId = 1 };
                var package = new Packages { PackagesId = 1, Height = 1, Length = 1, Weight = 1, Width = 1 };
                var deliveryCars = new List<DeliveryCars>
            {
                new DeliveryCars { DeliveryCarsId = 1, CityId = 1, MaxHeight = 10, MaxLength = 10, MaxWeight = 10, MaxWidth = 10, DeliveryCarStatus = DeliveryCarStatus.Free, AssigmentStatus = AssigmentStatus.Assigned },
                new DeliveryCars { DeliveryCarsId = 2, CityId = 1, MaxHeight = 10, MaxLength = 10, MaxWeight = 10, MaxWidth = 10, DeliveryCarStatus = DeliveryCarStatus.Free, AssigmentStatus = AssigmentStatus.Assigned }
            };
                var deliveryOrders = new List<DeliveryCarOrder>
            {
                new DeliveryCarOrder { DeliveryCarId = 1, DeliveryStatus = DeliveryStatusEnum.Pending, OrderId = 1 },
                new DeliveryCarOrder { DeliveryCarId = 1, DeliveryStatus = DeliveryStatusEnum.Pending, OrderId = 2 },
                new DeliveryCarOrder { DeliveryCarId = 2, DeliveryStatus = DeliveryStatusEnum.Pending, OrderId = 3 },
                new DeliveryCarOrder { DeliveryCarId = 2, DeliveryStatus = DeliveryStatusEnum.Pending, OrderId = 4 }
            };

                _mockOrdersService.Setup(x => x.GetPackagesByPackageIdIdAsync(order.PackagesId)).ReturnsAsync(package);
                _mockRepositoryWrapper.Setup(x => x.DeliveryCarsRepository.FindByCondition(It.IsAny<Expression<Func<DeliveryCars, bool>>>()))
                    .Returns(deliveryCars.AsQueryable());
                _mockRepositoryWrapper.Setup(x => x.DeliveryRepository.FindByCondition(It.IsAny<Expression<Func<DeliveryCarOrder, bool>>>()))
                    .Returns(deliveryOrders.AsQueryable());

                var result = await _deliveryService.CreateOrUpdateDeliveryWithOrder(order);

                Assert.IsTrue(result);
            }

            [TestMethod]
            public async Task CreateOrUpdateDeliveryWithOrder_OneDeliveryBiggerDifferenceInOrdersThanTheOther()
            {
                var order = new Order { CityId = 1, PackagesId = 1, OrderId = 1 };
                var package = new Packages { PackagesId = 1, Height = 1, Length = 1, Weight = 1, Width = 1 };
                var deliveryCars = new List<DeliveryCars>
            {
                new DeliveryCars { DeliveryCarsId = 1, CityId = 1, MaxHeight = 10, MaxLength = 10, MaxWeight = 10, MaxWidth = 10, DeliveryCarStatus = DeliveryCarStatus.Free, AssigmentStatus = AssigmentStatus.Assigned },
                new DeliveryCars { DeliveryCarsId = 2, CityId = 1, MaxHeight = 10, MaxLength = 10, MaxWeight = 10, MaxWidth = 10, DeliveryCarStatus = DeliveryCarStatus.Free, AssigmentStatus = AssigmentStatus.Assigned }
            };
                var deliveryOrders = new List<DeliveryCarOrder>
            {
                new DeliveryCarOrder { DeliveryCarId = 1, DeliveryStatus = DeliveryStatusEnum.Pending, OrderId = 1 },
                new DeliveryCarOrder { DeliveryCarId = 1, DeliveryStatus = DeliveryStatusEnum.Pending, OrderId = 2 },
                new DeliveryCarOrder { DeliveryCarId = 1, DeliveryStatus = DeliveryStatusEnum.Pending, OrderId = 3 },
                new DeliveryCarOrder { DeliveryCarId = 1, DeliveryStatus = DeliveryStatusEnum.Pending, OrderId = 4 },
                new DeliveryCarOrder { DeliveryCarId = 2, DeliveryStatus = DeliveryStatusEnum.Pending, OrderId = 5 },
                new DeliveryCarOrder { DeliveryCarId = 2, DeliveryStatus = DeliveryStatusEnum.Pending, OrderId = 6 }
            };

                _mockOrdersService.Setup(x => x.GetPackagesByPackageIdIdAsync(order.PackagesId)).ReturnsAsync(package);
                _mockRepositoryWrapper.Setup(x => x.DeliveryCarsRepository.FindByCondition(It.IsAny<Expression<Func<DeliveryCars, bool>>>()))
                    .Returns(deliveryCars.AsQueryable());
                _mockRepositoryWrapper.Setup(x => x.DeliveryRepository.FindByCondition(It.IsAny<Expression<Func<DeliveryCarOrder, bool>>>()))
                    .Returns(deliveryOrders.AsQueryable());

                var result = await _deliveryService.CreateOrUpdateDeliveryWithOrder(order);
                Assert.IsTrue(result);
            }
        }
    }
}
