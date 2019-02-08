using System;
using System.Security.Claims;

namespace VehicleTracking.Common
{
	public interface IToken
	{
		string CreateToken(string secretKey, string issuer, Claim[] claims, DateTime expireTime);
		string GetValue(string token, string claimName);
	}
}
