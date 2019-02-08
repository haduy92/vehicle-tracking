using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Application.Modules.Commands
{
	public class CreateTrackingRecordCommand : BaseRequest, IRequest
	{
		public string VehicleCode { get; set; }
		public string DeviceCode { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public DateTime RecordedDate { get; set; }

		public class Handler : IRequestHandler<CreateTrackingRecordCommand>
		{
			private readonly IUnitOfWork _unitOfWork;
			private readonly IMediator _mediator;

			public Handler(IUnitOfWork unitOfWork, IMediator mediator)
			{
				_unitOfWork = unitOfWork;
				_mediator = mediator;
			}

			public async Task<Unit> Handle(CreateTrackingRecordCommand request, CancellationToken cancellationToken)
			{
				// Check if vehicle and device code exist
				Guid vehicleId = await _unitOfWork.VehicleRepository
					.GetQueryableAsNoTracking(x => x.VehicleCode.ToLower() == request.VehicleCode.ToLower()
						&& x.DeviceCode.ToLower() == request.DeviceCode.ToLower()
						&& x.IsActive)
					.Select(x => x.Id)
					.SingleOrDefaultAsync(cancellationToken);

				if (vehicleId == null || vehicleId == Guid.Empty)
				{
					throw new NotFoundException(nameof(Vehicle), request.VehicleCode);
				}

				var trackingRecord = new TrackingRecord
				{
					DeviceCode = request.DeviceCode,
					Latitude = request.Latitude,
					Longitude = request.Longitude,
					VehicleCode = request.VehicleCode,
					RecordedDate = request.RecordedDate
				};

				// Get lastest snapshot id of vehicle
				var snapshot = await _unitOfWork.TrackingSnapshotRepository
					.GetQueryable(
						filter: x => x.VehicleId == vehicleId && x.RecordedDate == request.RecordedDate.Date,
						orderBy: x => x.OrderByDescending(p => p.RecordedDate))
					.SingleOrDefaultAsync(cancellationToken);

				if (snapshot == null)
				{
					// Create new instance of snapshot
					snapshot = _unitOfWork.TrackingSnapshotRepository.Create(new TrackingRecordSnapshot
					{
						VehicleId = vehicleId,
						RecordedDate = request.RecordedDate.Date
					});
				}

				trackingRecord.TrackingRecordSnapshotId = snapshot.Id;

				_unitOfWork.TrackingRepository.Create(trackingRecord);
				_unitOfWork.Commit();

				return Unit.Value;
			}
		}
	}
}
