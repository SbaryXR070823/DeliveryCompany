using DeliveryCompany.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models.ViewModels;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly DataAppDbContext _dbContext;
        public OrdersService(DataAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<OrderVM>> GetOrdersAsync(string userId)
        {
			List<OrderVM> orderViewModels = new List<OrderVM>();

			var ordersList = await _dbContext.Orders
				.Include(o => o.Packages) 
				.Where(o => o.UserId == userId)
				.ToListAsync();

			foreach (var order in ordersList)
			{
				OrderVM orderViewModel = new OrderVM
				{
					Id = order.OrderId,
					Price = order.Price,
					Name = order.Packages.Name,
					Description = order.Packages.Description,
					Weight = order.Packages.Weight,
					Width = order.Packages.Width,
					Length = order.Packages.Length,
					Height = order.Packages.Height,
					DateTime = order.DateTime,
					Status = order.OrderStatus
				};
				orderViewModels.Add(orderViewModel);
			}

			return orderViewModels;
		}
    }
}
