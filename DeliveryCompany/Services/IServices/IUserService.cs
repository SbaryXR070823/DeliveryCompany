using DeliveryCompany.Models.ViewModels;
using Models.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
	public interface IUserService
	{
		Task<List<UserRoleInfo>> GetAllUsersFromRole(string role);
		Task<bool> CreateUser(UserCreationVM userCreation);
	}
}
