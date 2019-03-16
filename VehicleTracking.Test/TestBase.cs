using Microsoft.EntityFrameworkCore;
using System;
using VehicleTracking.Persistence;

namespace VehicleTracking.Test
{
	public class TestBase
	{
		protected readonly VehicleTrackingDbContext _context;

		public TestBase()
		{
			_context = GetContext();
		}

		private VehicleTrackingDbContext GetContext()
		{
			var builder = new DbContextOptionsBuilder<VehicleTrackingDbContext>();

			builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

			VehicleTrackingDbContext dbContext = new VehicleTrackingDbContext(builder.Options);

			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			
			return dbContext;
		}
	}
}
