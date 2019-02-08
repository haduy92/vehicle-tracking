using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VehicleTracking.Persistence.Infrastructure
{
	public interface IDbContext : IDisposable
	{
		ChangeTracker ChangeTracker { get; }
		DbSet<TEntity> Set<TEntity>() where TEntity : class;
		int SaveChanges(bool acceptAllChangesOnSuccess = true);
		Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default(CancellationToken));
		EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
		void EnsureDatabaseCreated();
	}
}
