using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.ValueObjects;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Application.Modules.Commands
{
	public class CreateUserCommand : BaseRequest, IRequest
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public string Password { get; set; }
		
		// Address properties
		public string StreetAddress1 { get; set; }
		public string StreetAddress2 { get; set; }
		public string StreetAddress3 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }

		public class Handler : IRequestHandler<CreateUserCommand, Unit>
		{
			private readonly IUnitOfWork _unitOfWork;
			private readonly IPassword _password;
            private readonly IMediator _mediator;

			public Handler(IUnitOfWork unitOfWork,
				IPassword password,
				IMediator mediator)
			{
				_unitOfWork = unitOfWork;
				_password = password;
				_mediator = mediator;
			}

			public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
			{
				var isDuplicatedEmail = _unitOfWork.UserRepository
					.GetQueryableAsNoTracking(x => x.EmailAddress == request.EmailAddress)
					.AnyAsync(cancellationToken);

				if (await isDuplicatedEmail)
				{
					throw new DuplicatedException(nameof(EmailAddress), request.EmailAddress);
				}

				byte[] passwordHash, passwordSalt;
				_password.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

				var entity = new User
				{
					FirstName = request.FirstName,
					LastName = request.LastName,
					PasswordHash = passwordHash,
					PasswordSalt = passwordSalt,
					Address = new Address(request.StreetAddress1, request.StreetAddress2, request.StreetAddress3, request.City,
						request.State, request.Country, request.ZipCode)
				};

				_unitOfWork.UserRepository.Create(entity);
				_unitOfWork.Commit();

				return Unit.Value;
			}
		}
	}
}
