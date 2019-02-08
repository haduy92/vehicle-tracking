using System;
using VehicleTracking.Common;

namespace VehicleTracking.Infrastructure
{
	public class MachineDateTime : IDateTime
	{
		public DateTime Now => DateTime.UtcNow;
	}
}
