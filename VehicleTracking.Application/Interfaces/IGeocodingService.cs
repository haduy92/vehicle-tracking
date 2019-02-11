using System.Threading.Tasks;

namespace VehicleTracking.Application.Interfaces
{
	public interface IGeocodingService
	{
		/// <summary>
		/// Return the locality name of matching latitude and longitude.
		/// </summary>
		/// <param name="lat">Latitude</param>
		/// <param name="lng">Longitute</param>
		/// <param name="key">Google Map API Key</param>
		Task<string> ReverseGeocoding(string lat, string lng, string key);
	}
}
