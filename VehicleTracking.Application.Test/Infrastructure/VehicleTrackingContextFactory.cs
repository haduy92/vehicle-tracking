using Microsoft.EntityFrameworkCore;
using System;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence;

namespace VehicleTracking.Application.Test.Infrastructure
{
	public class VehicleTrackingContextFactory
	{
		public static VehicleTrackingDbContext Create()
		{
			var options = new DbContextOptionsBuilder<VehicleTrackingDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			var context = new VehicleTrackingDbContext(options);

			context.Database.EnsureCreated();

			context.Users.AddRange(new[] {
				new User { FirstName = "John", LastName = "Smith", Email = "johnsmith@email.com" },
				new User { FirstName = "Lois", LastName = "Lane", Email = "linalane@email.com" },
				new User { FirstName = "Peter", LastName = "Parker", Email = "peterparker@email.com" }
			});

			context.SaveChanges();

			return context;
		}

		public static void Destroy(VehicleTrackingDbContext context)
		{
			context.Database.EnsureDeleted();

			context.Dispose();
		}
	}
}
