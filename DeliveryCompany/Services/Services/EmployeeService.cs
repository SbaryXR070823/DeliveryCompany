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
   //         var lastEmployee = _repositoryWrapper.EmployeeRepository
			//	.FindAll()
			//	.OrderByDescending(x => x.EmployeeId)
			//	.FirstOrDefault();
			//if (lastEmployee != null)
			//{
			//	employee.EmployeeId = lastEmployee.EmployeeId++;
			//}
			//else
			//{
			//	employee.EmployeeId = 1;
			//}
			employee.AssigmentStatus = DeliveryCompany.Utility.Enums.AssigmentStatus.Unassigned;
			_repositoryWrapper.EmployeeRepository.Create(employee);
			_repositoryWrapper.Save();
		}
	}
}
