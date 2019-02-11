using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using VehicleTracking.Application.Interfaces;

namespace VehicleTracking.Infrastructure
{
	public class GeocodingService : IGeocodingService
	{
		public async Task<string> ReverseGeocoding(string lat, string lng, string key)
		{
			string requestUri = "https://maps.googleapis.com/maps/api/geocode/xml";
			string parameters = $"?latlng={lat},{lng}&key={key}";

			XmlDocument doc = new XmlDocument();

			using (var client = new HttpClient())
			{
				var request = await client.GetAsync(requestUri + parameters);
				var content = request.Content.ReadAsStringAsync();

				doc.LoadXml(await content);
			}

			XmlNode element = doc.SelectSingleNode("//GeocodeResponse/status");
			if (element.InnerText.ToLower() == "ok")
			{
				element = doc.SelectSingleNode("//GeocodeResponse/result/formatted_address");
				return element.InnerText;
			}

			return string.Empty;
		}
	}
}
