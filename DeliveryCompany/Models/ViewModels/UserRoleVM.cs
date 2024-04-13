using Models.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
	public class UserRoleVM
	{
		public List<UserRoleInfo> AdminUsers { get; set; }
		public List<UserRoleInfo> EmployeeUsers { get; set; }
		public List<UserRoleInfo> RegularUsers { get; set; }

	}
}
