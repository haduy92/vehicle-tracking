using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.Enumerations;

namespace VehicleTracking.Persistence.Infrastructure
{
	public class AuditEntry
	{
		private readonly IDateTime _dateTime;

		public AuditEntry(EntityEntry entry, IDateTime dateTime)
		{
			_dateTime = dateTime;
			Entry = entry;
		}

		public EntityEntry Entry { get; }
		public string TableName { get; set; }
		public AuditAction Action { get; set; }
		public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
		public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
		public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
		public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

		public bool HasTemporaryProperties => TemporaryProperties.Any();

		public AuditLog ToAuditLog()
		{
			var audit = new AuditLog();
			audit.TableName = TableName;
			audit.CreatedDate = _dateTime.Now;
			audit.KeyValues = JsonConvert.SerializeObject(KeyValues);
			audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
			audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
			audit.Action = Action;
			return audit;
		}
	}
}
