using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.Models;
using DeliveryCompany.Services.IServices;
using Repository.IRepository;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
	public class EmployeeService : IEmployeeService
	{
		private IRepositoryWrapper _repositoryWrapper;
		public EmployeeService(IRepositoryWrapper repositoryWrapper)
		{
			_repositoryWrapper = repositoryWrapper;
		}

		public async Task AddNewEMployee(EmployeeToAdd employeeToAdd)
		{
			Employee employee = new Employee();
            employee.Name = employeeToAdd.Name;
            employee.UserId = employeeToAdd.UserId;
			employee.AssigmentStatus = DeliveryCompany.Utility.Enums.AssigmentStatus.Unassigned;
			_repositoryWrapper.EmployeeRepository.Create(employee);
			_repositoryWrapper.Save();
		}

        public async Task DeleteEmployeeByUserId(string userId)
        {
            var employee = _repositoryWrapper.EmployeeRepository.FindByCondition(x => x.UserId.Equals(userId)).FirstOrDefault();
            if (employee != null)
            {
                _repositoryWrapper.EmployeeRepository.Delete(employee);
                _repositoryWrapper.Save();
            }
        }
    }
}
