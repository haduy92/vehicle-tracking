using VehicleTracking.Domain.ValueObjects;
using Xunit;

namespace VehicleTracking.Domain.Test.ValueObjects
{
	public class AddressTests
	{
		[Fact]
		public void ToString_ReturnsCorrectString()
		{
			const string value = "3423 Dane Street Basin City Washington 99343 US";

			var address = new Address("3423 Dane", "Basin", "Washington", "99343", "US");

			Assert.Equal(value, address.ToString());
		}

		[Fact]
		public void Implicit_ReturnCorrectString()
		{
			const string value = "3423 Dane Street Basin City Washington 99343 US";

			var address = new Address("3423 Dane", "Basin", "Washington", "99343", "US");

			string result = address;

			Assert.Equal(value, result);
		}
	}
}
