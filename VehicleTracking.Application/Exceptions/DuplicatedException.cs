using System;

namespace VehicleTracking.Application.Exceptions
{
	public class DuplicatedException : Exception
	{
		public DuplicatedException(string name, object key)
            : base($"Entity \"{name}\" (\"{key}\") was duplicated.")
        {
        }
	}
}
