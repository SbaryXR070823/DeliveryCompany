using Constants.Authentification;
using Models.Authentification;
using DeliveryCompany.Models.Models;
using DeliveryCompany.Models.ViewModels;
using DeliveryCompany.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Services.IServices;

namespace DeliveryCompany.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmployeeService _employeeService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public UserManagementController(IUserService userService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IEmployeeService employeeService)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _employeeService = employeeService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            UserRoleVM allUsers = new UserRoleVM();
            var users = await _userService.GetAllUsersFromRole(UserRoles.User);
            var admins = await _userService.GetAllUsersFromRole(UserRoles.Admin);
            var employees = await _userService.GetAllUsersFromRole(UserRoles.DeliveryEmployee);
            allUsers.AdminUsers = admins;
            allUsers.EmployeeUsers = employees;
            allUsers.RegularUsers = users;
            return View(allUsers);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(UserCreationVM userCreation)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUser(userCreation);
                if (result)
                {
                    if (userCreation.Role == UserRoles.DeliveryEmployee)
                    {
                        var user = await _userManager.FindByEmailAsync(userCreation.Email);
                        EmployeeToAdd employeeToAdd = new EmployeeToAdd
                        {
                            Name = userCreation.Name,
                            UserId = user.Id
                        };
                        await _employeeService.AddNewEMployee(employeeToAdd);
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(userCreation);
        }
    }
}
