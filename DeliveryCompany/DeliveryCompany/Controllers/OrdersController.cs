using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Utility.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.ViewModels;
using Services.IServices;
using Utility.Helpers;

namespace DeliveryCompany.Controllers
{
	public class OrdersController : Controller
	{
		private readonly IOrdersService _ordersService;

		public OrdersController(IOrdersService ordersService)
		{
			_ordersService = ordersService;
		}
		public async Task<IActionResult> IndexAsync()
		{
			var orders = await _ordersService.GetOrdersAsync(User.GetUserId());
			return View(orders);
		}

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync(OrderVM orderView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.GetUserId();
                    Packages package = new Packages
                    {
                        Width = orderView.Width,
                        Height = orderView.Height,
                        Name = orderView.Name,
                        Description = orderView.Description,
                        Weight = orderView.Weight,
                        Length = orderView.Length
                    };
                    await _ordersService.CreateOrderAsync(package, userId);
                    RedirectToAction("Index", "Orders");
                }
                return View(orderView);

            }
            catch (Exception)
            {
                RedirectToAction("Index", "Orders");
                return View();
            }
        }
        [HttpPost]
        public async Task DeleteAsync(int id)
        {
            var isDeleted = await _ordersService.DeleteAsync(id);
            if (!isDeleted)
            {
                RedirectToAction("Index", "Orders");
            }
            RedirectToAction("Index", "Orders");
        }
    }
}
