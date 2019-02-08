using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace VehicleTracking.Domain.Infrastructure
{
	public abstract class ValueObject
	{
		protected static bool EqualOperator(ValueObject left, ValueObject right)
		{
			if (left is null ^ right is null)
			{
				return false;
			}

			return left?.Equals(right) != false;
		}

		protected static bool NotEqualOperator(ValueObject left, ValueObject right)
		{
			return !(EqualOperator(left, right));
		}

		protected abstract IEnumerable<object> GetAtomicValues();

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}

			var other = obj as ValueObject;
			var thisValues = GetAtomicValues().GetEnumerator();
			var otherValues = other.GetAtomicValues().GetEnumerator();

			while (thisValues.MoveNext() && otherValues.MoveNext())
			{
				if (thisValues.Current is null ^ otherValues.Current is null)
				{
					return false;
				}

				if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
				{
					return false;
				}
			}

			return !thisValues.MoveNext() && !otherValues.MoveNext();
		}

		public override int GetHashCode()
		{
			return GetAtomicValues()
				.Select(x => x != null ? x.GetHashCode() : 0)
				.Aggregate((x, y) => x ^ y);
		}

		public bool IsEmpty()
		{
			Type t = GetType();
			FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			foreach (FieldInfo field in fields)
			{
				object value = field.GetValue(this);
				if (value != null)
				{
					return false;
				}
			}
			return true;
		}
	}
}
