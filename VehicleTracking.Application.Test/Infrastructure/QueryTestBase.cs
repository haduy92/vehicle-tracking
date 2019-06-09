using System;
using VehicleTracking.Persistence;

namespace VehicleTracking.Application.Test.Infrastructure
{
	public class QueryTestBase : IDisposable
	{
		protected readonly VehicleTrackingDbContext _context;

		public QueryTestBase()
		{
			_context = VehicleTrackingContextFactory.Create();
		}

		public void Dispose()
		{
			VehicleTrackingContextFactory.Destroy(_context);
		}
	}
}
