using DeliveryCompany.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
	public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
	{
		protected DataAppDbContext _appDbContext { get; set; }

		public RepositoryBase(DataAppDbContext locationContext)
		{
			this._appDbContext = locationContext;
		}

		public IQueryable<T> FindAll()
		{
			return this._appDbContext.Set<T>().AsNoTracking();
		}

		public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
		{
			return this._appDbContext.Set<T>().Where(expression);
		}

		public void Create(T entity)
		{
			this._appDbContext.Set<T>().Add(entity);
		}

		public void Update(T entity)
		{
			this._appDbContext.Set<T>().Update(entity);
		}

		public void Delete(T entity)
		{
			this._appDbContext.Set<T>().Remove(entity);
		}
	}
}
