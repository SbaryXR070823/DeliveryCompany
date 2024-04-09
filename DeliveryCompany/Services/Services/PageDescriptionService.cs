using DeliveryCompany.DataAccess.Data;
using DeliveryCompany.Models.DbModels;
using DeliveryCompany.Services.IServices;
using DeliveryCompany.Utility.Enums;
using Repository.IRepository;


namespace DeliveryCompany.Services.Services
{

	public class PageDescriptionService : IPageDescriptionService
	{
		private IRepositoryWrapper _repositoryWrapper;
		public PageDescriptionService(IRepositoryWrapper repositoryWrapper)
		{
			_repositoryWrapper = repositoryWrapper;
		}

		public async Task<Models.DbModels.PageDescriptions> GetPageDescription(int Id)
		{
			return _repositoryWrapper.PageDescriptionRepository.FindByCondition(x => x.PageDescriptionsId.Equals(Id)).FirstOrDefault();
		}

		public async Task UpdatePageDescription(Models.DbModels.PageDescriptions pageDescriptions)
		{
			_repositoryWrapper.PageDescriptionRepository.Update(pageDescriptions);
			_repositoryWrapper.Save();
		}
	}
}
