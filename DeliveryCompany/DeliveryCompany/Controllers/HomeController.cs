using DeliveryCompany.Models;
using Models.ViewModels;
using DeliveryCompany.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DeliveryCompany.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICityService _cityService;
        private readonly IPageDescriptionService _pageDescriptionService;

        public HomeController(ILogger<HomeController> logger, ICityService cityService, IPageDescriptionService pageDescriptionService)
        {
            _cityService = cityService;
            _logger = logger;
            _pageDescriptionService = pageDescriptionService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var cities = await _cityService.GetCitiesAsync();
            var pageDescription = await _pageDescriptionService.GetPageDescription((int)Utility.Enums.PageDescriptions.HomePage);
            HomePageVM view = new HomePageVM
            {
                Cities = cities,
                PageDescriptions = pageDescription
            };
            return View(view);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
