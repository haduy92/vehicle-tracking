using System;
using VehicleTracking.Domain.Exceptions;
using VehicleTracking.Domain.ValueObjects;
using Xunit;

namespace VehicleTracking.Domain.Test.ValueObjects
{
	public class EmailTests
	{
		[Fact]
		public void Validation_GivenEmptyString_ShouldReturnInvalidEmailException()
		{
			Action act = () => new Email("");

			Assert.Throws<InvalidEmailException>(act);
		}

		[Fact]
		public void Validation_GivenIncorrectFormat_ShouldReturnInvalidEmailException()
		{
			Action act = () => new Email("username@");

			Assert.Throws<InvalidEmailException>(act);
		}

		[Fact]
		public void ToString_ReturnsCorrectString()
		{
			const string value = "username@company.com";

			var email = new Email(value);

			Assert.Equal(value, email.ToString());
		}

		[Fact]
		public void Implicit_ReturnCorrectString()
		{
			const string value = "username@company.com";

			var email = new Email(value);

			string result = email;

			Assert.Equal(value, result);
		}

		[Fact]
		public void Implicit_ReturnCorrectEmailObject()
		{
			const string value = "username@company.com";

			Email result = value;

			Assert.Equal(value, result.Value);
		}
	}
}
