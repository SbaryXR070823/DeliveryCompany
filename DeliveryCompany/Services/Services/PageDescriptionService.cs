using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Utility.Enums;


namespace DeliveryCompany.Services.Services
{

    public class PageDescriptionService : IPageDescriptionService
    {
        private readonly DataAppDbContext _dbContext;
        public PageDescriptionService(DataAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Models.DbModels.PageDescriptions> GetPageDescription(int Id)
        {
            return await _dbContext.PageDescriptions.FindAsync(Id);
        }
    }
}
