using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Services.IServices
{
    public interface IPageDescriptionService
    {
        Task<Models.DbModels.PageDescriptions> GetPageDescription(int Id);
		Task UpdatePageDescription(Models.DbModels.PageDescriptions pageDescriptions);

	}
}
