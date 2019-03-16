using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Application.Modules.Models;
using VehicleTracking.Persistence;

namespace VehicleTracking.Application.Modules.Queries
{
	public class GetUserListQuery : BaseRequest, IRequest<UserListViewModel>
	{
		public int PageNumb { get; set; } = 0;
		public int PageSize { get; set; } = 10;

		public class Handler : IRequestHandler<GetUserListQuery, UserListViewModel>
		{
			private readonly VehicleTrackingDbContext _context;

			public Handler(VehicleTrackingDbContext context)
			{
				_context = context;
			}

			public async Task<UserListViewModel> Handle(GetUserListQuery request, CancellationToken cancellationToken)
			{
				var viewModels = await _context.Users
					.AsNoTracking()
					.Skip(request.PageSize* request.PageNumb)
					.Take(request.PageSize)
					.OrderBy(p => p.FirstName).ThenBy(p => p.LastName)
					.Select(UserViewModel.Projection)
					.ToListAsync(cancellationToken);

				// TODO: Set view model state based on user permissions.
				var model = new UserListViewModel()
				{
					Users = viewModels,
					CreateEnabled = true
				};

				return model;
			}
		}
	}
}
