using System;

namespace VehicleTracking.Application.Exceptions
{
	public class CreateFailureException : Exception
	{
		public CreateFailureException(string name, object key)
			: base($"Creation of entity \"{name}\" (\"{key}\") failed.")
		{ }
	}
}
