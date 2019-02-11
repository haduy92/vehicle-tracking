using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Extensions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Application.Modules.Commands
{
	public class DeleteUserCommand : BaseRequest, IRequest
	{
		public string Id { get; set; }

		public class Handler : IRequestHandler<DeleteUserCommand, Unit>
		{
			private readonly IUnitOfWork _unitOfWork;
			private readonly IMediator _mediator;

			public Handler(IUnitOfWork unitOfWork, IMediator mediator)
			{
				_unitOfWork = unitOfWork;
				_mediator = mediator;
			}

			public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
			{
				var guid = request.Id.ToGUID();
				var entity = await _unitOfWork.UserRepository
					.GetQueryableAsNoTracking(x => x.Id == guid)
					.FirstOrDefaultAsync();

				if (entity == null)
				{
					throw new NotFoundException(nameof(User), request.Id);
				}

				_unitOfWork.UserRepository.Delete(entity);
				await _unitOfWork.CommitAsync(true, cancellationToken);

				return Unit.Value;
			}
		}
	}
}
