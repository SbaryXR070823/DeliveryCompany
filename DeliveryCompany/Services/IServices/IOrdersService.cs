using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.Models;
using DeliveryCompany.Utility.Enums;
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
		Task<List<OrderVM>> GetOrdersAsync(string userId=null);
        Task<Order> CreateOrderAsync(Packages package, UserOrderInformations userOrderInformations);
        Task<bool> DeleteAsync(int id);
        Task<OrderVM> GetCitiesWithOrderViewModel();
        Task<Packages> GetPackagesByPackageIdIdAsync(int packageId);
        Task UpdateStatusOfOrder(int orderId, OrderStatus orderStatus);


    }
}
