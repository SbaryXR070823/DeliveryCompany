using Constants.Authentification;
using DeliveryCompany.Models.ViewModels;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Utility.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Authentification;
using Services.IServices;
using Utility.Helpers;

namespace DeliveryCompany.Controllers;

[Authorize]
public class DeliveriesController : Controller
{
    private readonly IDeliveryService _deliveryService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmployeeService _employeeService;
    private readonly IDeliveryCarsService _deliveryCarsService;
    private readonly IOrdersService _ordersService;
    public DeliveriesController(IDeliveryService deliveryService, UserManager<AppUser> userManager, IEmployeeService employeeService, IDeliveryCarsService deliveryCarsService, IOrdersService ordersService)
    {
        _deliveryService = deliveryService;
        _userManager = userManager;
        _employeeService = employeeService;
        _deliveryCarsService = deliveryCarsService;
        _ordersService = ordersService;
    }
    [Authorize]


    public async Task<IActionResult> IndexAsync()
    {
        var employee = _employeeService.GetEmployeeByUserId(User.GetUserId());
        var deliveryCar = _deliveryCarsService.GetDeliveryCarByEmployeeId(employee.EmployeeId);
        if (deliveryCar is null)
        {
            return View(new List<DeliveryOrdersVM>());
        }
        var deliveries = await _deliveryService.GetOrdersByCarId(deliveryCar.DeliveryCarsId);
        return View(deliveries);
    }

    [HttpPost]
    public async Task UpdateOrderStatusAsync(int id, OrderStatus status)
    {
        await _ordersService.UpdateStatusOfOrder(id, status);
        RedirectToAction("Index");
    }

    [HttpPost]
    public async Task UpdateDeliveryStatusAsync(int id, DeliveryStatusEnum status)
    {
        await _deliveryService.UpdateDeliveries(id, status);
        RedirectToAction("Index");
    }
}
