using System;

namespace VehicleTracking.Domain.Entities
{
	public class TrackingRecord : BaseEntity
	{
		public string VehicleCode { get; set; }
		public string DeviceCode { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }

		/// <summary>
		/// Actual time of device calling API, to distinguish with CreatedDate property.
		/// </summary>
		public DateTime RecordedDate { get; set; }
		public Guid TrackingRecordSnapshotId { get; set; }

		public TrackingRecordSnapshot TrackingRecordSnapshot { get; set; }
	}
}
