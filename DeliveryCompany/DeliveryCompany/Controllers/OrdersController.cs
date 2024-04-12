using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Models.Models;
using DeliveryCompany.Utility.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Authentification;
using Models.ViewModels;
using Services.IServices;
using Utility.Helpers;

namespace DeliveryCompany.Controllers
{
	public class OrdersController : Controller
	{
        private readonly IOrdersService _ordersService;
        private readonly UserManager<AppUser> _userManager;
        public OrdersController(IOrdersService ordersService, UserManager<AppUser> userManager)
        {
            _ordersService = ordersService;
            _userManager = userManager;
        }
        [HttpGet]
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
                    var user = await _userManager.GetUserAsync(User);
                    Packages package = new Packages
                    {
                        Width = orderView.Width,
                        Height = orderView.Height,
                        Name = orderView.Name,
                        Description = orderView.Description,
                        Weight = orderView.Weight,
                        Length = orderView.Length,

                    };
                    UserOrderInformations userOrderInformations = new UserOrderInformations
                    {
                        UserId = User.GetUserId(),
                        UserAddress = user.Address,
                        UserCityId = orderView.CityId
                    };

                    await _ordersService.CreateOrderAsync(package, userOrderInformations);
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
