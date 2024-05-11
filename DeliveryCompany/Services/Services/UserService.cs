using Constants.Authentification;
using Models.Authentification;
using DeliveryCompany.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Services.IServices;
using Serilog;


namespace Services.Services
{
	public class UserService : IUserService
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;

		public UserService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		public async Task<List<UserRoleInfo>> GetAllUsersFromRole(string role)
		{
			Log.Information("Retrieving all users for role {0}...",role);
			var users = await _userManager.GetUsersInRoleAsync(role);
			List<UserRoleInfo> usersToReturn = new List<UserRoleInfo>();
			foreach (var user in users)
			{
				UserRoleInfo userRoleInfo = new UserRoleInfo();
				userRoleInfo.Role = role;
				userRoleInfo.Email = user.Email;
				userRoleInfo.Address = user.Address;
				usersToReturn.Add(userRoleInfo);
				userRoleInfo.UserId = user.Id;
			}
			return usersToReturn;
		}

		public async Task<bool> CreateUser(UserCreationVM userCreation)
		{
            AppUser appUser = new AppUser
            {
                Name = userCreation.Name,
                Email = userCreation.Email,
                UserName = userCreation.Email,
                Address = userCreation.Address,
            };
			Log.Information("Creating user {@0}...", appUser);
            var result = await _userManager.CreateAsync(appUser, userCreation.Password);
			Log.Information("Adding {0} to role {1}...",appUser.Email, userCreation.Role);
            var roleAssigmentResult = await _userManager.AddToRoleAsync(appUser, userCreation.Role);
            if (result.Succeeded && roleAssigmentResult.Succeeded)
            {
				Log.Information("Succesgfully created user {@0} and added it to role {1}...", appUser, userCreation.Role);
				return true;
            }

			return false;
        }


    }
}
