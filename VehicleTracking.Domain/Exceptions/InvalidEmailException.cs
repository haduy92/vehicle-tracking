using System;

namespace VehicleTracking.Domain.Exceptions
{
	public class InvalidEmailException : Exception
	{
		public InvalidEmailException(string email)
			: base($"Email \"{email}\" is invalid.")
		{
		}
	}
}
