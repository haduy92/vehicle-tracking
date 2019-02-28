using Microsoft.EntityFrameworkCore;
using System;
using VehicleTracking.Infrastructure;
using VehicleTracking.Persistence;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Test
{
	public class TestBase
	{
		protected readonly IUnitOfWork _unitOfWork;

		public TestBase()
		{
			_unitOfWork = GetUnitOfWork();
		}

		private IUnitOfWork GetUnitOfWork()
		{
			var builder = new DbContextOptionsBuilder<VehicleTrackingDbContext>();

			builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

			VehicleTrackingDbContext dbContext = new VehicleTrackingDbContext(builder.Options);

			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			
			return new UnitOfWork(dbContext, new MachineDateTime());
		}
	}
}
