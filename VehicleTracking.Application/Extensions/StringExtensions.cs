using System;

namespace VehicleTracking.Application.Extensions
{
	public static class StringExtensions
	{
		public static bool IsNullOrEmpty(this string s)
		{
			return string.IsNullOrEmpty(s);
		}

		public static Guid ToGUID(this string s)
		{
			Guid guid = Guid.Empty;

			if (!s.IsNullOrEmpty())
			{
				Guid.TryParse(s, out guid);
			}

			return guid;
		}
	}
}
