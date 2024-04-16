using DeliveryCompany.Models.ViewModels;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryCompany.Controllers
{
    [Route("[controller]")]
    public class DeliveryCarsController : Controller
    {
        private readonly IDeliveryCarsService _deliveryCarsService;
        private readonly ICityService _cityService;
        private readonly IEmployeeService _employeeService;
        public DeliveryCarsController(IDeliveryCarsService ordersService, ICityService cityService, IEmployeeService employeeService)
        {
            _deliveryCarsService = ordersService;
            _cityService = cityService;
            _employeeService = employeeService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var deliveryCars = _deliveryCarsService.GetAllCarsAsync();
            var cities = await _cityService.GetCitiesAsync();
            var deliveryCarsVM = new DeliveryCarsVM
            {
                Cars = deliveryCars,
                Cities = cities
            };
            return View(deliveryCarsVM);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task DeleteAsync(int id)
        {
            await _deliveryCarsService.DeleteDeliveryCarById(id);
            RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync(int cityId)
        {
            var employess = await _employeeService.GetAllEmployeesByStatus(Utility.Enums.AssigmentStatus.Unassigned);
            DeliveryCarCreationVM deliveryCarsVM = new DeliveryCarCreationVM
            {
                CityId = cityId,
                Employees = employess
            };
            return View(deliveryCarsVM);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync(DeliveryCarCreationVM deliveryCarCreationVM)
        {
            if (ModelState.IsValid)
            {
                await _deliveryCarsService.AddNewDeliveryCar(deliveryCarCreationVM);
                return RedirectToAction("Index");
            }
            return View(deliveryCarCreationVM);
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> EditAsync(int id)
        {
            var deliveryCar = await _deliveryCarsService.GetDeliveryCarById(id);
            var employees = await _employeeService.GetAllEmployeesByStatus(Utility.Enums.AssigmentStatus.Unassigned);
            var alreadyAssignedEmployee = _employeeService.GetEmployeeById((int)deliveryCar.EmployeeId);
            employees.Add(alreadyAssignedEmployee);
            DeliveryCarCreationVM deliveryCarCreationVM = new DeliveryCarCreationVM
            {
                MaxHeight = deliveryCar.MaxHeight,
                MaxLength = deliveryCar.MaxLength,
                MaxWeight = deliveryCar.MaxWeight,
                MaxWidth = deliveryCar.MaxWidth,
                EmployeeId = deliveryCar.EmployeeId,
                CityId = deliveryCar.CityId,
                DeliveryCarsId = deliveryCar.DeliveryCarsId,
                Employees = employees,
            };
            return View(deliveryCarCreationVM);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditAsync(DeliveryCarCreationVM deliveryCarCreationVM)
        {
            if (ModelState.IsValid)
            {
                await _deliveryCarsService.UpdateDeliveryCar(deliveryCarCreationVM);
                return RedirectToAction("Index");
            }
            return View(deliveryCarCreationVM);
        }
    }
}
