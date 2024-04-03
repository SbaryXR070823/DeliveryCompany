using DeliveryCompany.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCompany.Models.ViewModels
{
    public class HomePageViewModel
    {
        public List<City> Cities { get; set; }
        public PageDescriptions PageDescriptions { get; set; }
    }
}
