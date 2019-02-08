namespace VehicleTracking.Domain.Entities
{
	public class Vehicle : BaseEntity
	{
		public string VehicleCode { get; set; }
		public string DeviceCode { get; set; }
		public bool IsActive { get; set; }

		/// <summary>
		/// Storing all extra fields in an XML structure.
		/// </summary>
		/// <remarks>
		/// Use XPath/XQuery to retrieve from database.
		/// </remarks>
		public string ExtendedData { get; set; }
	}
}
