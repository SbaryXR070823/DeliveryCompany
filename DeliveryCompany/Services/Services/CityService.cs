using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

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
			_repositoryWrapper.Save();
		}

		public async Task DeleteCity(int id)
		{
			var city = _repositoryWrapper.CityRepository.FindByCondition(x => x.CityId == id).FirstOrDefault();
			_repositoryWrapper.CityRepository.Delete(city);
			_repositoryWrapper.Save();
		}
	}
}
