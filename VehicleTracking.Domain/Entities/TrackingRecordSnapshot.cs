using System;
using System.Collections.Generic;

namespace VehicleTracking.Domain.Entities
{
	public class TrackingRecordSnapshot : BaseEntity
	{
		public TrackingRecordSnapshot() : base()
		{
			TrackingRecords = new HashSet<TrackingRecord>();
		}

		public Guid VehicleId { get; set; }
		public DateTime RecordedDate { get; set; }
		public ICollection<TrackingRecord> TrackingRecords { get; private set; }
	}
}
