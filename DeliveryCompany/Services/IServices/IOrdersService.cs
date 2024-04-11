using DeliveryCompany.Models.DbModels;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IOrdersService
    {
		Task<List<OrderVM>> GetOrdersAsync(string userId);
        Task CreateOrderAsync(Packages package, string userId, string address);
        Task<bool> DeleteAsync(int id);


    }
}
