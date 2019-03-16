using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Extensions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Application.Modules.Models;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence;

namespace VehicleTracking.Application.Modules.Queries
{
	public class GetUserByIdQuery : BaseRequest, IRequest<UserViewModel>
	{
		public string Id { get; set; }

		public class Handler : IRequestHandler<GetUserByIdQuery, UserViewModel>
		{
			private readonly VehicleTrackingDbContext _context;

			public Handler(VehicleTrackingDbContext context)
			{
				_context = context;
			}

			public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
			{
				var guid = request.Id.ToGUID();

				var user = await _context.Users
					.AsNoTracking()
					.SingleOrDefaultAsync(x => x.Id == guid, cancellationToken);

				if (user == null)
				{
					throw new NotFoundException(nameof(User), request.Id);
				}

				return UserViewModel.Create(user);
			}
		}
	}
}
