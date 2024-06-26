﻿using DeliveryCompany.DataAccess.Data;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
	public class RepositoryWrapper : IRepositoryWrapper
	{
		private DataAppDbContext _appDbContext;
		private ICityRepository? _cityRepository;
		private IOrderRepository? _orderRepository;
		private IPageDescriptionRepository? _pageDescriptionRepository;
		private IPackageRepository? _packageRepository;
		private IEmployeeRepository? _employeeRepository;
		private IDeliveryCarsRepository? _deliveryCarsRepository;
        private IDeliveryRepository? _deliveryRepository;


        public ICityRepository CityRepository
		{
			get
			{
				if (_cityRepository == null)
				{
					_cityRepository = new CityRepository(_appDbContext);
				}

				return _cityRepository;
			}
		}

		public IOrderRepository OrderRepository
		{
			get
			{
				if (_orderRepository == null)
				{
					_orderRepository = new OrderRepository(_appDbContext);
				}

				return _orderRepository;
			}
		}

		public IPageDescriptionRepository PageDescriptionRepository
		{
			get
			{
				if (_pageDescriptionRepository == null)
				{
					_pageDescriptionRepository = new PageDescriptionRepository(_appDbContext);
				}

				return _pageDescriptionRepository;
			}
		}

		public IPackageRepository PackageRepository
		{
			get
			{
				if (_packageRepository == null)
				{
					_packageRepository = new PackageRepository(_appDbContext);
				}

				return _packageRepository;
			}
		}

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                if (_employeeRepository == null)
                {
                    _employeeRepository = new EmployeeRepository(_appDbContext);
                }

                return _employeeRepository;
            }
        }

		public IDeliveryCarsRepository DeliveryCarsRepository
		{
			get
			{
				if (_deliveryCarsRepository == null)
				{
					_deliveryCarsRepository = new DeliveryCarsRepository(_appDbContext);
				}

				return _deliveryCarsRepository;
			}
		}

        public IDeliveryRepository DeliveryRepository
        {
            get
            {
                if (_deliveryRepository == null)
                {
                    _deliveryRepository = new DeliveryRepository(_appDbContext);
                }

                return _deliveryRepository;
            }
        }


        public RepositoryWrapper(DataAppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public void Save()
		{
			_appDbContext.SaveChanges();
		}
	}
}
