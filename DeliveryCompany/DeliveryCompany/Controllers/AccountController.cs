using Constants.Authentification;
using Models.Authentification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace DeliveryCompany.Controllers;
public class AccountController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync(Login loginModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.Username!, loginModel.Password!, loginModel.RemembeMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid login attempt");
            return View(loginModel);
        }
        return View(loginModel);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(Register registerModel)
    {
        if (ModelState.IsValid)
        {
            AppUser appUser = new AppUser
            {
                Name = registerModel.Name,
                Email = registerModel.Email,
                UserName = registerModel.Email,
                Address = registerModel.Address,
            };

            var result = await _userManager.CreateAsync(appUser, registerModel.Password);
            var roleAssigmentResult = await _userManager.AddToRoleAsync(appUser, UserRoles.User);

            if (result.Succeeded && roleAssigmentResult.Succeeded)
            {
                await _signInManager.SignInAsync(appUser, false);
                return RedirectToAction("Index", "Home");
            }
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            if (!roleAssigmentResult.Succeeded)
            {

                foreach (var error in roleAssigmentResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }
        return View(registerModel);
    }


    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
