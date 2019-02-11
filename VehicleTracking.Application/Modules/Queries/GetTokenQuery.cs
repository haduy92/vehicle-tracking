using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Extensions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Helpers;

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
			private readonly IUnitOfWork _unitOfWork;

			public Handler(IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public async Task<string> Handle(GetTokenQuery request, CancellationToken cancellationToken)
			{
				var user = await _unitOfWork.UserRepository
					.GetQueryableAsNoTracking(x => x.EmailAddress == request.EmailAddress)
					.SingleOrDefaultAsync(cancellationToken);

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
					_unitOfWork.Commit();
				}				

				return user.Token;
			}
		}
	}
}
