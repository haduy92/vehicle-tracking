using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Persistence
{
	public class VehicleTrackingDbContextFactory : DesignTimeDbContextFactoryBase<VehicleTrackingDbContext>
	{
		protected override VehicleTrackingDbContext CreateNewInstance(string connectionString)
		{
			return new VehicleTrackingDbContext(connectionString);
		}
	}
}
