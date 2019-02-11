using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace VehicleTracking.Application.Helpers
{
	public class TokenHelper
	{
		/// <summary>
		/// Return a token string.
		/// </summary>
		/// <param name="secretKey">Secret key</param>
		/// <param name="issuer">Issuer name</param>
		/// <param name="claims">Claims list</param>
		/// <param name="expireTime">Token expire time</param>
		public static string CreateToken(string secretKey, string issuer, Claim[] claims, DateTime expireTime)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			
			var token = new JwtSecurityToken(issuer, issuer, claims, expires: expireTime, signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		/// <summary>
		/// Get claim value from token string by name.
		/// </summary>
		/// <param name="token">Token string</param>
		/// <param name="claimName">Claim name</param>
		public static string GetValue(string token, string claimName)
		{
			var handler = new JwtSecurityTokenHandler();
			var tokenS = handler.ReadToken(token) as JwtSecurityToken;

			return tokenS.Claims.FirstOrDefault(claim => claim.Type == claimName)?.Value;
		}
	}
}
