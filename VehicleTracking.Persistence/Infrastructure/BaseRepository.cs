using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence.Infrastructure
{
	public interface IBaseRepository<T> : IDisposable where T : BaseEntity
	{
		IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			string includeProperties = null,
			int? skip = null,
			int? take = null);
		IQueryable<T> GetQueryableAsNoTracking(Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			string includeProperties = null,
			int? skip = null,
			int? take = null);

		T Create(T entity);
		Task CreateAsync(T entity);
		void CreateMany(ICollection<T> entities);
		Task CreateManyAsync(ICollection<T> entities);
		void Update(T entity);
		void Delete(object id);
		void Delete(T entity);
	}

	public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
	{
		private readonly IDbContext _dbContext;
		private readonly IDateTime _dateTime;
		private readonly DbSet<T> _dbSet;
		private bool _disposed = false;

		protected BaseRepository(IDbContext dbContext, IDateTime dateTime)
		{
			_dbContext = dbContext;
			_dateTime = dateTime;
			_dbSet = dbContext.Set<T>();
		}

		public virtual IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			string includeProperties = null,
			int? skip = null,
			int? take = null)
		{
			includeProperties = includeProperties ?? string.Empty;
			IQueryable<T> query = _dbSet.AsQueryable();

			if (filter != null)
			{
				query = query.Where(filter);
			}

			foreach (var includeProperty in includeProperties.Split
				(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProperty);
			}

			if (orderBy != null)
			{
				query = orderBy(query);
			}

			if (skip.HasValue)
			{
				query = query.Skip(skip.Value);
			}

			if (take.HasValue)
			{
				query = query.Take(take.Value);
			}

			return query;
		}

		public virtual IQueryable<T> GetQueryableAsNoTracking(Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			string includeProperties = null,
			int? skip = null,
			int? take = null)
		{
			return GetQueryable(filter, orderBy, includeProperties, skip, take).AsNoTracking();
		}

		public virtual T Create(T entity)
		{
			entity.CreatedDate = _dateTime.Now;
			return _dbSet.Add(entity).Entity;
		}

		public virtual Task CreateAsync(T entity)
		{
			entity.CreatedDate = _dateTime.Now;
			return _dbSet.AddAsync(entity);
		}

		public virtual void CreateMany(ICollection<T> entities)
		{
			foreach (var entity in entities)
			{
				entity.CreatedDate = _dateTime.Now;
			}
			_dbSet.AddRange(entities);
		}

		public virtual Task CreateManyAsync(ICollection<T> entities)
		{
			foreach (var entity in entities)
			{
				entity.CreatedDate = _dateTime.Now;
			}
			return _dbSet.AddRangeAsync(entities);
		}

		public virtual void Update(T entity)
		{
			_dbSet.Attach(entity);
			_dbContext.Entry(entity).State = EntityState.Modified;
		}

		public virtual void Delete(object id)
		{
			T entity = _dbSet.Find(id);
			Delete(entity);
		}

		public virtual void Delete(T entity)
		{
			if (_dbContext.Entry(entity).State == EntityState.Detached)
			{
				_dbSet.Attach(entity);
			}
			_dbSet.Remove(entity);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_dbContext.Dispose();
				}
			}
			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}	
}
