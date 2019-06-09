using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Helpers;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Application.Modules.Notifications;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.ValueObjects;
using VehicleTracking.Persistence;

namespace VehicleTracking.Application.Modules.Commands
{
	public class CreateUserCommand : BaseRequest, IRequest
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		
		// Address properties
		public string StreetAddress1 { get; set; }
		public string StreetAddress2 { get; set; }
		public string StreetAddress3 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string Country { get; set; }

		public class Handler : IRequestHandler<CreateUserCommand, Unit>
		{
			private readonly VehicleTrackingDbContext _context;
            private readonly IMediator _mediator;

			public Handler(VehicleTrackingDbContext context, IMediator mediator)
			{
				_context = context;
				_mediator = mediator;
			}

			public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
			{
				var isDuplicatedEmail = _context.Users
					.AsNoTracking()
					.AnyAsync(x => x.Email == request.Email, cancellationToken);

				if (await isDuplicatedEmail)
				{
					throw new DuplicatedException(nameof(Email), request.Email);
				}

				byte[] passwordHash, passwordSalt;
				PasswordHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

				var entity = new User
				{
					FirstName = request.FirstName,
					LastName = request.LastName,
					Email = request.Email,
					PasswordHash = passwordHash,
					PasswordSalt = passwordSalt,
					Address = new Address(request.StreetAddress1, request.StreetAddress2, request.StreetAddress3, request.City,
						request.State, request.Country, request.ZipCode),
				};

				_context.Users.Add(entity);
				_context.SaveChanges();

				await _mediator.Publish(new UserCreated { Id = entity.Id.ToString(), Email = entity.Email });

				return Unit.Value;
			}
		}
	}
}
