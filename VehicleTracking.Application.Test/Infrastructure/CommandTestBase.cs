using VehicleTracking.Persistence;

namespace VehicleTracking.Application.Test.Infrastructure
{
	public class CommandTestBase
	{
		protected readonly VehicleTrackingDbContext _context;

		public CommandTestBase()
		{
			_context = VehicleTrackingContextFactory.Create();
		}

		public void Dispose()
		{
			VehicleTrackingContextFactory.Destroy(_context);
		}
	}
}
