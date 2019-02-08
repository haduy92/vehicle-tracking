using System.Linq;
using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Persistence
{
	public class VehicleTrackingDataSeeder
	{
		public static void Initialize(IUnitOfWork unitOfWork, IPassword password)
		{
			var seeder = new VehicleTrackingDataSeeder();
			seeder.SeedAll(unitOfWork, password);
		}

		public void SeedAll(IUnitOfWork unitOfWork, IPassword password)
		{
			if (unitOfWork.UserRepository.GetQueryable().Any())
			{
				return; // Db has been seeded
			}

			SeedVehicles(unitOfWork);
			SeedUsers(unitOfWork, password);
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

		private void SeedUsers(IUnitOfWork unitOfWork, IPassword password)
		{
			byte[] passwordHash, passwordSalt;
			password.CreatePasswordHash("admin@123", out passwordHash, out passwordSalt);

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
