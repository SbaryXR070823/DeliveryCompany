using DeliveryCompany.Models;
using Models.ViewModels;
using DeliveryCompany.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DeliveryCompany.Models.DbModels;
using static DeliveryCompany.Constants.Misc.MiscConstants;

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

		public async Task UpdateHomeDescription([FromBody] PageDescriptions pageDescriptions)
		{
            try
            {
                await _pageDescriptionService.UpdatePageDescription(pageDescriptions);
                TempData[TempDataValues.Success] = "The Home page description was updated successfully!";
                RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData[TempDataValues.Error] = $"There was an error when updating the Home page description! Error: {ex.Message}";
                RedirectToAction("Index");
                throw;
            }
        }

		[HttpPost]
		public async Task AddNewCity([FromBody] string name)
		{
            try
            {
                TempData[TempDataValues.Success] = "The City was added successfully!";
                await _cityService.AddNewCity(name);
            }
            catch (Exception ex)
            {
                TempData[TempDataValues.Error] = $"There was an error when adding the City! Error: {ex.Message}";
                throw;
            }
        }

		public async Task DeleteCity(int id)
		{
            try
            {
                TempData[TempDataValues.Success] = "The City was deleted successfully!";
                await _cityService.DeleteCity(id);
            }
            catch (Exception ex)
            {
                TempData[TempDataValues.Error] = $"There was an error when deleting the City! Error: {ex.Message}";
                throw;
            }
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
