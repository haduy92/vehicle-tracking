using System.Collections.Generic;
using System.Text.RegularExpressions;
using VehicleTracking.Domain.Exceptions;
using VehicleTracking.Domain.Infrastructure;

namespace VehicleTracking.Domain.ValueObjects
{
	public class Email : ValueObject
	{
		public string Value { get; }

		private Email() { }

		public Email(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new InvalidEmailException(value);
			}

			if (!Regex.IsMatch(value, @"^(.+)@(.+)$"))
			{
				throw new InvalidEmailException(value);
			}

			Value = value;
		}

		public static implicit operator string(Email email)
		{
			return email.Value;
		}

		public static implicit operator Email(string value)
		{
			return new Email(value);
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Value;
		}

		public override string ToString() => Value;
	}
}
