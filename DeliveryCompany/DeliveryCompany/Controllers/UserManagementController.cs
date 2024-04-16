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


        [HttpPost]
        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var isEmployee = await _userManager.IsInRoleAsync(user, UserRoles.DeliveryEmployee);
            if (isEmployee)
            {
                await _employeeService.DeleteEmployeeByUserId(user.Id);
            }
            await _userManager.DeleteAsync(user);
            RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userRole = await _userManager.GetRolesAsync(user);
            UserEditVM employeeToAdd = new UserEditVM
            {
                Name = user.Name,
                Address = user.Address,
                Email = user.Email,
                OldEmail = user.Email,
                Role = userRole.FirstOrDefault(),
            };
            return View(employeeToAdd);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(UserEditVM userToEdit)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userToEdit.OldEmail);
                var userRole = await _userManager.GetRolesAsync(user);
                user.Email = userToEdit.Email;
                user.Name = userToEdit.Name;
                user.Address = userToEdit.Address;
                if (userToEdit.Role != userRole.FirstOrDefault())
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole.FirstOrDefault());
                    await _userManager.AddToRoleAsync(user, userToEdit.Role);
                    if (userRole.FirstOrDefault() == UserRoles.DeliveryEmployee)
                    {
                        await _employeeService.DeleteEmployeeByUserId(user.Id);
                    }

                    if (userToEdit.Role == UserRoles.DeliveryEmployee)
                    {
                        EmployeeToAdd employeeToAdd = new EmployeeToAdd
                        {
                            Name = user.Name,
                            UserId = user.Id,
                        };
                        await _employeeService.AddNewEMployee(employeeToAdd);
                    }
                }

                if (!string.IsNullOrEmpty(userToEdit.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, userToEdit.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(userToEdit);
                    }
                }
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            return View(userToEdit);
        }
    }
}
