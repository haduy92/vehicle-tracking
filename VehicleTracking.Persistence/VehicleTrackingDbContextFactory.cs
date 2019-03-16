using Microsoft.EntityFrameworkCore;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Persistence
{
	public class VehicleTrackingDbContextFactory : DesignTimeDbContextFactoryBase<VehicleTrackingDbContext>
	{
		protected override VehicleTrackingDbContext CreateNewInstance(DbContextOptions<VehicleTrackingDbContext> options)
		{
			return new VehicleTrackingDbContext(options);
		}
	}
}
