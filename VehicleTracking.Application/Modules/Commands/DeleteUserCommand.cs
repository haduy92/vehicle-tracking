using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Extensions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence;

namespace VehicleTracking.Application.Modules.Commands
{
	public class DeleteUserCommand : BaseRequest, IRequest
	{
		public string Id { get; set; }

		public class Handler : IRequestHandler<DeleteUserCommand, Unit>
		{
			private readonly VehicleTrackingDbContext _context;
			private readonly IMediator _mediator;

			public Handler(VehicleTrackingDbContext context, IMediator mediator)
			{
				_context = context;
				_mediator = mediator;
			}

			public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
			{
				var guid = request.Id.ToGUID();
				var entity = await _context.Users
					.AsNoTracking()
					.FirstOrDefaultAsync(x => x.Id == guid);

				if (entity == null)
				{
					throw new NotFoundException(nameof(User), request.Id);
				}

				_context.Users.Remove(entity);
				await _context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
