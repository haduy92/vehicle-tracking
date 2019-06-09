using VehicleTracking.Domain.ValueObjects;

namespace VehicleTracking.Domain.Entities
{
	public class User : BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }
		public string Token { get; set; }
		public Email Email { get; set; }
		public Address Address { get; set; }
	}
}
