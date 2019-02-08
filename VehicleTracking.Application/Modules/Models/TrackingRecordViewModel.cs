using System;

namespace VehicleTracking.Application.Modules.Models
{
	public class TrackingRecordViewModel
	{
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public string LocalityName { get; set; }
		public DateTime RecordedDate { get; set; }
	}
}
