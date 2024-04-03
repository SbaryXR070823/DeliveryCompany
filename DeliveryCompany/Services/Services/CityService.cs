using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DeliveryCompany.Services.Services
{
    public class CityService : ICityService
    {
        private readonly DataAppDbContext _dbContext;
        public CityService(DataAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<City>> GetCitiesAsync()
        {
            var result = await _dbContext.Cities.OrderBy(c => c.Name).ToListAsync();
            return result;
        }

		public async Task AddNewCity(string name)
		{
			City city = new City { Name = name };
			var result = await _dbContext.Cities.AddAsync(city);
			if (result != null)
			{
				await _dbContext.SaveChangesAsync();
			}
		}

		public async Task DeleteCity(int id)
		{
			var city = await _dbContext.Cities.FindAsync(id);
			_dbContext.Remove(city);
			await _dbContext.SaveChangesAsync();
		}
	}
}
