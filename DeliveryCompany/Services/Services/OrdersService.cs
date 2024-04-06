using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Utility.Enums;
using Microsoft.EntityFrameworkCore;
using Models.ViewModels;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Helpers;

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

		public async Task CreateOrderAsync(Packages package, string userId)
		{
            Packages packageOrder = new Packages
            {
                Width = package.Width,
                Height = package.Height,
                Name = package.Name,
                Description = package.Description,
                Weight = package.Weight,
                Length = package.Length
            };

            await _dbContext.Packages.AddAsync(package);
            await _dbContext.SaveChangesAsync();

            Order order = new Order
            {
                OrderStatus = OrderStatus.Unassigned,
                DateTime = DateTime.Now,
                Price = OrderHelpers.CalculatePrice(package.Weight, package.Width, package.Length, package.Height),
                UserId = userId,
                Packages = package
            };

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order is null)
            {
                return false;
            }
            var package = await _dbContext.Packages.FindAsync(order.PackagesId);
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            _dbContext.Packages.Remove(package);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
