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
    }
}
