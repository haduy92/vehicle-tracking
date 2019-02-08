using VehicleTracking.Domain.ValueObjects;

namespace VehicleTracking.Domain.Entities
{
	public class User : BaseEntity
	{
		public User() : base()
		{
			Address = Address.Empty();
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }
		public string Token { get; set; }
		public Address Address { get; set; }
	}
}
