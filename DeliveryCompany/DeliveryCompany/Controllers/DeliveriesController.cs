using DeliveryCompany.Models.ViewModels;
using DeliveryCompany.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Authentification;
using Utility.Helpers;

namespace DeliveryCompany.Controllers;

[Authorize]
public class DeliveriesController : Controller
{
	private readonly IDeliveryService _deliveryService;
	private readonly UserManager<AppUser> _userManager;
	private readonly IEmployeeService _employeeService;
	private readonly IDeliveryCarsService _deliveryCarsService;
	public DeliveriesController(IDeliveryService deliveryService, UserManager<AppUser> userManager, IEmployeeService employeeService, IDeliveryCarsService deliveryCarsService)
	{
		_deliveryService = deliveryService;
		_userManager = userManager;
		_employeeService = employeeService;
		_deliveryCarsService = deliveryCarsService;
	}
	[Authorize]


    public async Task<IActionResult> IndexAsync()
    {
		var employee = _employeeService.GetEmployeeByUserId(User.GetUserId());
		var deliveryCar = _deliveryCarsService.GetDeliveryCarByEmployeeId(employee.EmployeeId);
		if(deliveryCar is null)
		{
			return View(new List<DeliveryOrdersVM>());
		}
		var deliveries = await _deliveryService.GetOrdersByCarId(deliveryCar.DeliveryCarsId);
        return View(deliveries);
    }
}
