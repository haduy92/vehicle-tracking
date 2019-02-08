using Microsoft.EntityFrameworkCore;
using VehicleTracking.Persistence.Configurations;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Persistence
{
	public class VehicleTrackingDbContext : DbContext, IDbContext
	{
		private readonly string _connString;

		public VehicleTrackingDbContext(DbContextOptions<VehicleTrackingDbContext> options)
			: base(options)
		{ }

		public VehicleTrackingDbContext(string connectionString)
		{
			_connString = connectionString;
		}

		public void EnsureDatabaseCreated()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_connString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(VehicleTrackingDbContext).Assembly);
		}
	}
}
