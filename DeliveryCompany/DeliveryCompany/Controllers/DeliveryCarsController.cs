using DeliveryCompany.Models.ViewModels;
using DeliveryCompany.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryCompany.Controllers
{
	public class DeliveryCarsController : Controller
	{
		private readonly IDeliveryCarsService _deliveryCarsService;
		public DeliveryCarsController(IDeliveryCarsService ordersService)
		{
			_deliveryCarsService = ordersService;
		}
		public IActionResult Index()
		{
			var deliveryCars = _deliveryCarsService.GetAllCarsAsync();
			var deliveryCarsVM = new DeliveryCarsVM
			{
				Cars = deliveryCars
			};
			return View(deliveryCarsVM);
		}

		[HttpPost]
		public async Task DeleteAsync(int id)
		{
			await _deliveryCarsService.DeleteDeliveryCarById(id);
			RedirectToAction("Index", "Orders");
		}
	}
}
