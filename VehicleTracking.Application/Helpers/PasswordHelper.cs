using System;
using System.Security.Cryptography;
using System.Text;

namespace VehicleTracking.Application.Helpers
{
	public class PasswordHelper
	{
		/// <summary>
		/// Create a hash and salt from password string in byte array.
		/// </summary>
		/// <param name="password">Password</param>
		/// <param name="passwordHash"></param>
		/// <param name="passwordSalt"></param>
		public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			}
		}
		
		/// <summary>
		/// Check if given password string match.
		/// </summary>
		/// <param name="password">Password</param>
		/// <param name="passwordHash"></param>
		/// <param name="passwordSalt"></param>
		public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
			if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
			if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

			using (var hmac = new HMACSHA512(storedSalt))
			{
				byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
				for (int i = 0; i < computedHash.Length; i++)
				{
					if (computedHash[i] != storedHash[i]) return false;
				}
			}

			return true;
		}
	}
}
