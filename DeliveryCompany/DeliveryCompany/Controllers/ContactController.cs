using DeliveryCompany.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Authentification;
using static DeliveryCompany.Constants.Misc.MiscConstants;
using Utility.Helpers;
using DeliveryCompany.Services.IServices;

namespace DeliveryCompany.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ContactController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(ContactVM contactMessage)
        {
            if (ModelState.IsValid)
            {
                contactMessage.UserId = User.GetUserId();
                var message = "Message: " + contactMessage.Message + "\nPhoneNumber: " + contactMessage.PhoneNumber + "\nEmail: " + contactMessage.Email + "\nUserId: " + contactMessage.UserId;
                await _emailSender.SendEmailAsync(contactMessage.Email, Contact.Subject, message);
                return RedirectToAction("Index");
            }
            return View(contactMessage);
        }
    }
}
