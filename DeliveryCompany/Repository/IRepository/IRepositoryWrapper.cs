using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
	public interface IRepositoryWrapper
	{
		ICityRepository CityRepository { get; }
		IPageDescriptionRepository PageDescriptionRepository { get; }
		IOrderRepository OrderRepository { get; }
		IPackageRepository PackageRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        void Save();
	}
}
