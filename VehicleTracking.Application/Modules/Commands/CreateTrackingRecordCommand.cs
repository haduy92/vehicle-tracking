using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence;

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
			private readonly VehicleTrackingDbContext _context;
			private readonly IMediator _mediator;
			private readonly IDateTime _dateTime;

			public Handler(VehicleTrackingDbContext context, IDateTime dateTime, IMediator mediator)
			{
				_context = context;
				_dateTime = dateTime;
				_mediator = mediator;
			}

			public async Task<Unit> Handle(CreateTrackingRecordCommand request, CancellationToken cancellationToken)
			{
				// Check if vehicle and device code exist
				Guid vehicleId = await _context.Vehicles
					.AsNoTracking()
					.Where(x => x.VehicleCode.ToLower() == request.VehicleCode.ToLower()
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
					RecordedDate = request.RecordedDate,
					CreatedDate = _dateTime.Now
				};

				// Get lastest snapshot id of vehicle
				var snapshot = await _context.TrackingRecordSnapshots
					.Where(x => x.VehicleId == vehicleId && x.RecordedDate == request.RecordedDate.Date)
					.OrderByDescending(x => x.RecordedDate)
					.SingleOrDefaultAsync(cancellationToken);

				if (snapshot == null)
				{
					// Create new instance of snapshot
					snapshot = _context.TrackingRecordSnapshots.Add(new TrackingRecordSnapshot
					{
						VehicleId = vehicleId,
						RecordedDate = request.RecordedDate.Date,
						CreatedDate = _dateTime.Now
					})
					.Entity;
				}

				trackingRecord.TrackingRecordSnapshotId = snapshot.Id;

				_context.TrackingRecords.Add(trackingRecord);
				_context.SaveChanges();

				return Unit.Value;
			}
		}
	}
}
