using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Extensions;
using VehicleTracking.Application.Helpers;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence;

namespace VehicleTracking.Application.Modules.Queries
{
	public class GetTokenQuery : BaseRequest, IRequest<string>
	{
		public string EmailAddress { get; set; }
		public string Password { get; set; }
		public string SecretKey { get; set; }
		public string Issuer { get; set; }

		public class Handler : IRequestHandler<GetTokenQuery, string>
		{
			private readonly VehicleTrackingDbContext _context;

			public Handler(VehicleTrackingDbContext context)
			{
				_context = context;
			}

			public async Task<string> Handle(GetTokenQuery request, CancellationToken cancellationToken)
			{
				var user = await _context.Users
					.AsNoTracking()
					.SingleOrDefaultAsync(x => x.Email == request.EmailAddress, cancellationToken);

				if (user == null)
				{
					throw new NotFoundException(nameof(User), request.EmailAddress);
				}

				if (!PasswordHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
				{
					return null;
				}

				if (user.Token.IsNullOrEmpty())
				{
					var claims = new[] 
					{
						new Claim("user_id", user.Id.ToString())
					};

					user.Token = TokenHelper.CreateToken(request.SecretKey, request.Issuer, claims, DateTime.UtcNow.AddHours(1));
					_context.SaveChanges();
				}				

				return user.Token;
			}
		}
	}
}
