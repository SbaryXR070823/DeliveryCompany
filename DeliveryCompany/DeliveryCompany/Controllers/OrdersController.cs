using DeliveryCompany.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
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
	}
}
