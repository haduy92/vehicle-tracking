using System.Collections.Generic;

namespace VehicleTracking.Application.Modules.Models
{
	public class VehicleTrackingRecordListViewModel
	{
		public string VehicleCode { get; set; }
		public IEnumerable<TrackingRecordViewModel> TrackingRecords { get; set; }
	}
}
