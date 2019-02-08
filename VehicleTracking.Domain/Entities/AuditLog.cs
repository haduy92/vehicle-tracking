using System;
using VehicleTracking.Domain.Enumerations;

namespace VehicleTracking.Domain.Entities
{
	public class AuditLog : BaseEntity
	{
		public string TableName { get; set; }
		public string KeyValues { get; set; }
		public string OldValues { get; set; }
		public string NewValues { get; set; }		
		public AuditAction Action { get; set; }
	}
}
