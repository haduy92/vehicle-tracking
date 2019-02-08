namespace VehicleTracking.Common
{
	public interface IPassword
	{
		void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
		bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
	}
}
