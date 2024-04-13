using DeliveryCompany.Models.Models;
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
    }
}
