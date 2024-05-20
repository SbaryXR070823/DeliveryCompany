using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.ViewModels;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Services.Services;
using DeliveryCompany.Utility.Enums;
using Moq;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class DeliveryCarServiceTestClass
    {
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private Mock<IEmployeeService> _mockEmployeeService;
        private DeliveryCarService _deliveryCarService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockEmployeeService = new Mock<IEmployeeService>();
            _deliveryCarService = new DeliveryCarService(_mockRepositoryWrapper.Object, _mockEmployeeService.Object);
        }

        [TestMethod]
        public async Task AddNewDeliveryCar_WithEmployeeId()
        {
            var deliveryCarCreationVM = new DeliveryCarCreationVM
            {
                MaxHeight = 150,
                MaxLength = 250,
                MaxWeight = 1200,
                MaxWidth = 150,
                CityId = 1,
                EmployeeId = 10
            };

            _mockEmployeeService.Setup(x => x.UpdateEmployeeAssigmentStatus((int)deliveryCarCreationVM.EmployeeId, AssigmentStatus.Assigned))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper.Setup(x => x.DeliveryCarsRepository.Create(It.IsAny<DeliveryCars>()));
            _mockRepositoryWrapper.Setup(x => x.Save());

            await _deliveryCarService.AddNewDeliveryCar(deliveryCarCreationVM);

            _mockEmployeeService.Verify(x => x.UpdateEmployeeAssigmentStatus((int)deliveryCarCreationVM.EmployeeId, AssigmentStatus.Assigned), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.DeliveryCarsRepository.Create(It.Is<DeliveryCars>(d =>
                d.MaxHeight == deliveryCarCreationVM.MaxHeight &&
                d.MaxLength == deliveryCarCreationVM.MaxLength &&
                d.MaxWeight == deliveryCarCreationVM.MaxWeight &&
                d.MaxWidth == deliveryCarCreationVM.MaxWidth &&
                d.CityId == deliveryCarCreationVM.CityId &&
                d.EmployeeId == deliveryCarCreationVM.EmployeeId &&
                d.DeliveryCarStatus == DeliveryCarStatus.Free &&
                d.AssigmentStatus == AssigmentStatus.Assigned)), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public async Task AddNewDeliveryCar_WithoutEmployeeId()
        {
            var deliveryCarCreationVM = new DeliveryCarCreationVM
            {
                MaxHeight = 150,
                MaxLength = 250,
                MaxWeight = 1200,
                MaxWidth = 150,
                CityId = 1,
                EmployeeId = null
            };

            _mockRepositoryWrapper.Setup(x => x.DeliveryCarsRepository.Create(It.IsAny<DeliveryCars>()));
            _mockRepositoryWrapper.Setup(x => x.Save());

            await _deliveryCarService.AddNewDeliveryCar(deliveryCarCreationVM);

            _mockEmployeeService.Verify(x => x.UpdateEmployeeAssigmentStatus(It.IsAny<int>(), It.IsAny<AssigmentStatus>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.DeliveryCarsRepository.Create(It.Is<DeliveryCars>(d =>
                d.MaxHeight == deliveryCarCreationVM.MaxHeight &&
                d.MaxLength == deliveryCarCreationVM.MaxLength &&
                d.MaxWeight == deliveryCarCreationVM.MaxWeight &&
                d.MaxWidth == deliveryCarCreationVM.MaxWidth &&
                d.CityId == deliveryCarCreationVM.CityId &&
                d.EmployeeId == null &&
                d.DeliveryCarStatus == DeliveryCarStatus.Free &&
                d.AssigmentStatus == AssigmentStatus.Unassigned)), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public async Task DeleteDeliveryCarById_WithEmployeeId()
        {
            int deliveryCarId = 1;
            int employeeId = 10;
            var deliveryCar = new DeliveryCars
            {
                DeliveryCarsId = deliveryCarId,
                EmployeeId = employeeId,
                CityId = 1
            };

            _mockRepositoryWrapper.Setup(x => x.DeliveryCarsRepository.FindByCondition(It.IsAny<Expression<Func<DeliveryCars, bool>>>()))
                .Returns(new List<DeliveryCars> { deliveryCar }.AsQueryable());
            _mockEmployeeService.Setup(x => x.UpdateEmployeeAssigmentStatus(employeeId, AssigmentStatus.Unassigned))
                .Returns(Task.CompletedTask);
            _mockRepositoryWrapper.Setup(x => x.DeliveryCarsRepository.Delete(deliveryCar));
            _mockRepositoryWrapper.Setup(x => x.Save());

            await _deliveryCarService.DeleteDeliveryCarById(deliveryCarId);

            _mockEmployeeService.Verify(x => x.UpdateEmployeeAssigmentStatus(employeeId, AssigmentStatus.Unassigned), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.DeliveryCarsRepository.Delete(deliveryCar), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }
    }
}
