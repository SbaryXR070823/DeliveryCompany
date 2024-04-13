using Constants.Authentification;
using Models.Authentification;
using DeliveryCompany.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Services.IServices;


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
            var result = await _userManager.CreateAsync(appUser, userCreation.Password);
            var roleAssigmentResult = await _userManager.AddToRoleAsync(appUser, userCreation.Role);
            if (result.Succeeded && roleAssigmentResult.Succeeded)
            {
				return true;
            }

			return false;
        }


    }
}
