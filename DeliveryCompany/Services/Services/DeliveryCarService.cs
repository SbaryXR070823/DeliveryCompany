using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.ViewModels;
using DeliveryCompany.Services.IServices;
using Repository.IRepository;


namespace DeliveryCompany.Services.Services
{
    public class DeliveryCarService : IDeliveryCarsService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IEmployeeService _employeeService;
        public DeliveryCarService(IRepositoryWrapper repositoryWrapper, IEmployeeService employeeService)
        {
            _repositoryWrapper = repositoryWrapper;
            _employeeService = employeeService;
        }

        public List<DeliveryCars> GetAllCarsAsync()
        {
            var deliveryCars = _repositoryWrapper.DeliveryCarsRepository.FindAll().ToList();
            return deliveryCars;
        }

        public async Task DeleteDeliveryCarById(int deliveryCarId)
        {
            var deliverCar = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(dc => dc.DeliveryCarsId.Equals(deliveryCarId)).FirstOrDefault();
            if (deliverCar.EmployeeId is not null)
            {
                await _employeeService.UpdateEmployeeAssigmentStatus((int)deliverCar.EmployeeId, Utility.Enums.AssigmentStatus.Unassigned);
            }
            _repositoryWrapper.DeliveryCarsRepository.Delete(deliverCar);
            _repositoryWrapper.Save();
        }

        public async Task AddNewDeliveryCar(DeliveryCarCreationVM deliveryCarCreationVM)
        {
            DeliveryCars deliveryCars = new DeliveryCars
            {
                MaxHeight = deliveryCarCreationVM.MaxHeight,
                MaxLength = deliveryCarCreationVM.MaxLength,
                MaxWeight = deliveryCarCreationVM.MaxWeight,
                MaxWidth = deliveryCarCreationVM.MaxWidth,
                CityId = deliveryCarCreationVM.CityId,
                DeliveryCarStatus = Utility.Enums.DeliveryCarStatus.Free,
            };
            if (deliveryCarCreationVM.EmployeeId is not null)
            {
                deliveryCars.EmployeeId = deliveryCarCreationVM.EmployeeId;
                deliveryCars.AssigmentStatus = Utility.Enums.AssigmentStatus.Assigned;
                await _employeeService.UpdateEmployeeAssigmentStatus((int)deliveryCars.EmployeeId, Utility.Enums.AssigmentStatus.Assigned);
            }
            else
            {
                deliveryCars.AssigmentStatus = Utility.Enums.AssigmentStatus.Unassigned;

            }
            _repositoryWrapper.DeliveryCarsRepository.Create(deliveryCars);
            _repositoryWrapper.Save();
        }

        public async Task<DeliveryCars> GetDeliveryCarById(int id)
        {
            var deliveryCar = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(x => x.DeliveryCarsId.Equals(id)).FirstOrDefault();
            return deliveryCar;
        }

        public async Task UpdateDeliveryCar(DeliveryCarCreationVM deliveryCarVM)
        {
            var deliveryCar = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(x => x.DeliveryCarsId.Equals(deliveryCarVM.DeliveryCarsId)).FirstOrDefault();
            deliveryCar.MaxLength = deliveryCarVM.MaxLength;
            deliveryCar.MaxWidth = deliveryCarVM.MaxWidth;
            deliveryCar.MaxWeight = deliveryCarVM.MaxWeight;
            deliveryCar.MaxWidth = deliveryCar.MaxWidth;
            if (!deliveryCar.EmployeeId.Equals(deliveryCarVM.EmployeeId))
            {
                await _employeeService.UpdateEmployeeAssigmentStatus((int)deliveryCar.EmployeeId, Utility.Enums.AssigmentStatus.Unassigned);
                await _employeeService.UpdateEmployeeAssigmentStatus((int)deliveryCarVM.EmployeeId, Utility.Enums.AssigmentStatus.Assigned);
            }
            deliveryCar.EmployeeId = deliveryCarVM.EmployeeId;
            _repositoryWrapper.DeliveryCarsRepository.Update(deliveryCar);
            _repositoryWrapper.Save();
        }

        public DeliveryCars GetDeliveryCarByEmployeeId(int employeeId)
        {
            var deliveryCar = _repositoryWrapper.DeliveryCarsRepository.FindByCondition(c => c.EmployeeId.Equals(employeeId)).FirstOrDefault();
            return deliveryCar;
        }
    }
}
