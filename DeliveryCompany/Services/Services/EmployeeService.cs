using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.Models;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Utility.Enums;
using Repository.IRepository;
using Serilog;
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
            Log.Information("Adding new employee {@0}...", employee);
			_repositoryWrapper.EmployeeRepository.Create(employee);
			_repositoryWrapper.Save();
		}

        public async Task DeleteEmployeeByUserId(string userId)
        {
            var employee = _repositoryWrapper.EmployeeRepository.FindByCondition(x => x.UserId.Equals(userId)).FirstOrDefault();
            if (employee != null)
            {
                Log.Information("Deleting employee {@0}...", employee);
                _repositoryWrapper.EmployeeRepository.Delete(employee);
                _repositoryWrapper.Save();
            }
        }

        public async Task UpdateEmployeeAssigmentStatus(int employeeId, AssigmentStatus assigmentStatus)
        {
            var employee = _repositoryWrapper.EmployeeRepository.FindByCondition(x => x.EmployeeId.Equals(employeeId)).FirstOrDefault();
            employee.AssigmentStatus = assigmentStatus;
            Log.Information("Updating Assigment status for {@0}...",employee);
            _repositoryWrapper.EmployeeRepository.Update(employee);
            _repositoryWrapper.Save();
        }

        public Employee GetEmployeeById(int employeeId)
        {
            Log.Information("Retrieving employee by employeeId {0}...", employeeId);
            var employee = _repositoryWrapper.EmployeeRepository.FindByCondition(x => x.EmployeeId.Equals(employeeId)).FirstOrDefault();
            return employee;
        }

        public async Task<List<Employee>> GetAllEmployeesByStatus(AssigmentStatus assigmentStatus)
        {
            Log.Information("Retrieving all employees by status {@0}", assigmentStatus);
            var employess = _repositoryWrapper.EmployeeRepository.FindByCondition(x => x.AssigmentStatus.Equals(assigmentStatus)).ToList();
            if (employess != null)
            {
                return employess;
            }
            return new List<Employee>();
        }

        public Employee GetEmployeeByUserId(string userId)
        {
            Log.Information("Retrieving employee by userId {0}...", userId);
            var employee = _repositoryWrapper.EmployeeRepository.FindByCondition(x => x.UserId.Equals(userId)).FirstOrDefault();
            return employee;
        }
    }
}
