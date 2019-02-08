﻿using System.Collections.Generic;
using VehicleTracking.Domain.Infrastructure;

namespace VehicleTracking.Domain.ValueObjects
{
	public sealed class Address : ValueObject
	{
		public string StreetAddress1 { get; private set; }
		public string StreetAddress2 { get; private set; }
		public string StreetAddress3 { get; private set; }
		public string City { get; private set; }
		public string State { get; private set; }
		public string Country { get; private set; }
		public string ZipCode { get; private set; }

		private Address() { }

		public Address(string streetAddress1, string city, string state, string country, string zipCode)
		{
			StreetAddress1 = streetAddress1;
			City = city;
			State = state;
			Country = country;
			ZipCode = zipCode;
		}

		public Address(string streetAddress1, string streetAddress2, string city, string state, string country, string zipcode)
		: this(streetAddress1, city, state, zipcode, country)
		{
			StreetAddress2 = streetAddress2;
		}

		public Address(string streetAddress1, string streetAddress2, string streetAddress3, string city, string state, string country, string zipcode)
		: this(streetAddress1, city, state, zipcode, country)
		{
			StreetAddress2 = streetAddress2;
			StreetAddress3 = streetAddress3;
		}

		public static Address Empty()
		{
			return new Address("", "", "", "", "");
		}
		public override string ToString()
		{
			return $"{StreetAddress1} {City} {State} {ZipCode} {Country}";
		}

		public static implicit operator string(Address address)
		{
			return address.ToString();
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return StreetAddress1;
			yield return StreetAddress2;
			yield return StreetAddress3;
			yield return City;
			yield return State;
			yield return Country;
			yield return ZipCode;
		}
	}
}
