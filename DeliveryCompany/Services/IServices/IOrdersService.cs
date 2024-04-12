using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.Models;
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
        Task CreateOrderAsync(Packages package, UserOrderInformations userOrderInformations);
        Task<bool> DeleteAsync(int id);

        Task<OrderVM> GetCitiesWithOrderViewModel();


    }
}
