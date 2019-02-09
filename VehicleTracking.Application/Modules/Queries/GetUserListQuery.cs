using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Application.Modules.Models;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Application.Modules.Queries
{
	public class GetUserListQuery : BaseRequest, IRequest<UserListViewModel>
	{
		public int PageNumb { get; set; } = 0;
		public int PageSize { get; set; } = 10;

		public class Handler : IRequestHandler<GetUserListQuery, UserListViewModel>
		{
			private readonly IUnitOfWork _unitOfWork;

			public Handler(IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public async Task<UserListViewModel> Handle(GetUserListQuery request, CancellationToken cancellationToken)
			{
				var viewModels = await _unitOfWork.UserRepository
					.GetQueryableAsNoTracking(
						orderBy: x => x.OrderBy(p => p.FirstName).ThenBy(p => p.LastName),
						skip: request.PageSize * request.PageNumb,
						take: request.PageSize)
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
