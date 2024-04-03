using DeliveryCompany.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Services.IServices
{
    public interface ICityService
    {
        Task<List<City>> GetCitiesAsync();
	}
}
