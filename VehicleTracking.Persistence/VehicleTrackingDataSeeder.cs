using System.Linq;
using System.Security.Cryptography;
using System.Text;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Persistence
{
	public class VehicleTrackingDataSeeder
	{
		public static void Initialize(IUnitOfWork unitOfWork)
		{
			var seeder = new VehicleTrackingDataSeeder();
			seeder.SeedAll(unitOfWork);
		}

		public void SeedAll(IUnitOfWork unitOfWork)
		{
			if (unitOfWork.UserRepository.GetQueryable().Any())
			{
				return; // Db has been seeded
			}

			SeedVehicles(unitOfWork);
			SeedUsers(unitOfWork);
		}

		private void SeedVehicles(IUnitOfWork unitOfWork)
		{
			var vehicles = new[]
			{
				new Vehicle { VehicleCode = "V001", DeviceCode = "D001", IsActive = true },
				new Vehicle { VehicleCode = "V002", DeviceCode = "D002", IsActive = true },
				new Vehicle { VehicleCode = "V003", DeviceCode = "D003", IsActive = false },
				new Vehicle { VehicleCode = "V004", DeviceCode = "D004", IsActive = true },
				new Vehicle { VehicleCode = "V005", DeviceCode = "D005", IsActive = false }
			};

			unitOfWork.VehicleRepository.CreateMany(vehicles);
			unitOfWork.Commit();
		}

		private void SeedUsers(IUnitOfWork unitOfWork)
		{
			byte[] passwordHash, passwordSalt;

			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("admin@123"));
			}

			unitOfWork.UserRepository.Create(new User
			{
				EmailAddress = "admin@mail.com",
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt,
				FirstName = "John",
				LastName = "Smith",
			});

			unitOfWork.Commit();
		}
	}
}
