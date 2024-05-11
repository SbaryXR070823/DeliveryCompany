using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using Serilog;

namespace DeliveryCompany.Services.Services
{
    public class CityService : ICityService
    {
		private IRepositoryWrapper _repositoryWrapper;
		public CityService(IRepositoryWrapper repositoryWrapper)
		{
			_repositoryWrapper = repositoryWrapper;
		}

		public async Task<List<City>> GetCitiesAsync()
        {
			var result = _repositoryWrapper.CityRepository.FindAll().OrderBy(x => x.Name).ToList();
			return result;
		}

		public async Task AddNewCity(string name)
		{
			City city = new City { Name = name };
			_repositoryWrapper.CityRepository.Create(city);
			Log.Information("New city {0} added!...", city.Name);
			_repositoryWrapper.Save();
		}

		public async Task DeleteCity(int id)
		{
			var city = _repositoryWrapper.CityRepository.FindByCondition(x => x.CityId == id).FirstOrDefault();
			_repositoryWrapper.CityRepository.Delete(city);
            Log.Information("City {0} deleted!...", city.Name);
            _repositoryWrapper.Save();
		}
	}
}
