using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.Models;
using DeliveryCompany.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Services.IServices
{
    public interface IEmployeeService
    {
        Task AddNewEMployee(EmployeeToAdd employeeToAdd);
        Task DeleteEmployeeByUserId(string userId);
        Task<List<Employee>> GetAllEmployeesByStatus(AssigmentStatus assigmentStatus);
        Task UpdateEmployeeAssigmentStatus(int employeeId, AssigmentStatus assigmentStatus);
        Employee GetEmployeeById(int employeeId);
    }
}
