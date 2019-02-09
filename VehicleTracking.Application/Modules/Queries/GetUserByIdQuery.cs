using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Extensions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Application.Modules.Models;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Application.Modules.Queries
{
	public class GetUserByIdQuery : BaseRequest, IRequest<UserViewModel>
	{
		public string Id { get; set; }

		public class Handler : IRequestHandler<GetUserByIdQuery, UserViewModel>
		{
			private readonly IUnitOfWork _unitOfWork;

			public Handler(IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
			{
				var guid = request.Id.ToGUID();

				var user = await _unitOfWork.UserRepository
					.GetQueryableAsNoTracking(x => x.Id == guid)
					.SingleOrDefaultAsync(cancellationToken);

				if (user == null)
				{
					throw new NotFoundException(nameof(User), request.Id);
				}

				return UserViewModel.Create(user);
			}
		}
	}
}
