using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence
{
	public class VehicleTrackingDataSeeder
	{
		public static void Initialize(VehicleTrackingDbContext dbContext)
		{
			var seeder = new VehicleTrackingDataSeeder();
			seeder.SeedEverything(dbContext);
		}

		public void SeedEverything(VehicleTrackingDbContext dbContext)
		{
			dbContext.Database.Migrate();
			dbContext.Database.EnsureCreated();

			if (dbContext.Users.Any())
			{
				return; // Db has been seeded
			}

			SeedVehicles(dbContext);
			SeedUsers(dbContext);
		}

		private void SeedVehicles(VehicleTrackingDbContext dbContext)
		{
			var vehicles = new[]
			{
				new Vehicle { VehicleCode = "V001", DeviceCode = "D001", IsActive = true, CreatedDate = DateTime.UtcNow },
				new Vehicle { VehicleCode = "V002", DeviceCode = "D002", IsActive = true, CreatedDate = DateTime.UtcNow },
				new Vehicle { VehicleCode = "V003", DeviceCode = "D003", IsActive = false, CreatedDate = DateTime.UtcNow },
				new Vehicle { VehicleCode = "V004", DeviceCode = "D004", IsActive = true, CreatedDate = DateTime.UtcNow },
				new Vehicle { VehicleCode = "V005", DeviceCode = "D005", IsActive = false, CreatedDate = DateTime.UtcNow }
			};

			dbContext.Vehicles.AddRange(vehicles);
			dbContext.SaveChanges();
		}

		private void SeedUsers(VehicleTrackingDbContext dbContext)
		{
			byte[] passwordHash, passwordSalt;

			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("admin@123"));
			}

			dbContext.Users.Add(new User
			{
				EmailAddress = "admin@mail.com",
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt,
				FirstName = "John",
				LastName = "Smith",
				CreatedDate = DateTime.UtcNow
			});

			dbContext.SaveChanges();
		}
	}
}
