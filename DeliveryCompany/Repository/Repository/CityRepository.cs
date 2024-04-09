using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Models.DbModels;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
	public class CityRepository : RepositoryBase<City>, ICityRepository
	{
		public CityRepository(DataAppDbContext locationContext)
			: base(locationContext)
		{
		}
	}
}
