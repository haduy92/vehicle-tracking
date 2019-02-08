using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using VehicleTracking.Common;

namespace VehicleTracking.Infrastructure
{
	public class TokenHelper : IToken
	{
		public string CreateToken(string secretKey, string issuer, Claim[] claims, DateTime expireTime)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			
			var token = new JwtSecurityToken(issuer, issuer, claims, expires: expireTime, signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string GetValue(string token, string claimName)
		{
			var handler = new JwtSecurityTokenHandler();
			var tokenS = handler.ReadToken(token) as JwtSecurityToken;

			return tokenS.Claims.FirstOrDefault(claim => claim.Type == claimName)?.Value;
		}
	}
}
