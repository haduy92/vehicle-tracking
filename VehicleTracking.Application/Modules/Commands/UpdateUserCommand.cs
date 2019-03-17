using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Extensions;
using VehicleTracking.Application.Helpers;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.ValueObjects;
using VehicleTracking.Persistence;

namespace VehicleTracking.Application.Modules.Commands
{
	public class UpdateUserCommand : BaseRequest, IRequest
	{
		public string Id { get; set; }
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

		public class Handler : IRequestHandler<UpdateUserCommand, Unit>
		{
			private readonly VehicleTrackingDbContext _context;
			private readonly IDateTime _dateTime;
			private readonly IMediator _mediator;

			public Handler(VehicleTrackingDbContext context, IDateTime dateTime, IMediator mediator)
			{
				_context = context;
				_dateTime = dateTime;
				_mediator = mediator;
			}

			public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
			{
				var guid = request.Id.ToGUID();
				var entity = await _context.Users.SingleOrDefaultAsync(x => x.Id == guid);

				if (entity == null)
				{
					throw new NotFoundException(nameof(User), request.Id);
				}

				entity.FirstName = request.FirstName;
				entity.LastName = request.LastName;
				entity.Address = new Address(request.StreetAddress1, request.StreetAddress2, request.StreetAddress3, 
					request.City, request.State, request.Country, request.ZipCode);
				entity.UpdatedDate = _dateTime.Now;

				// update password if it was entered
				if (!request.Password.IsNullOrEmpty())
				{
					byte[] passwordHash, passwordSalt;
					PasswordHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

					entity.PasswordHash = passwordHash;
					entity.PasswordSalt = passwordSalt;
				}

				await _context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
