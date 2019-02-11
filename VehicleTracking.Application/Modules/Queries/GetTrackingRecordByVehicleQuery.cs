using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Modules.Models;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Application.Modules.Queries
{
	public class GetTrackingRecordByVehicleQuery : BaseRequest, IRequest<VehicleTrackingRecordViewModel>
	{
		public string VehicleCode { get; set; }
		public string DeviceCode { get; set; }
		public string APIKey { get; set; }

		public class Handler : IRequestHandler<GetTrackingRecordByVehicleQuery, VehicleTrackingRecordViewModel>
		{
			private readonly IUnitOfWork _unitOfWork;
			private readonly IGeocodingService _geocodingService;

			public Handler(IUnitOfWork unitOfWork, IGeocodingService geocodingService)
			{
				_unitOfWork = unitOfWork;
				_geocodingService = geocodingService;
			}

			public async Task<VehicleTrackingRecordViewModel> Handle(GetTrackingRecordByVehicleQuery request, CancellationToken cancellationToken)
			{
				// Check if input vehicle and device code exist
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

				// Get last vehicle tracking record
				var lastTrackingRecord = await _unitOfWork.TrackingSnapshotRepository
					.GetQueryableAsNoTracking(
						filter: x => x.VehicleId == vehicleId,
						orderBy: x => x.OrderByDescending(p => p.RecordedDate))
					.SelectMany(x => x.TrackingRecords)
					.OrderByDescending(x => x.RecordedDate)
					.Select(x => new VehicleTrackingRecordViewModel
					{
						Latitude = x.Latitude,
						Longitude = x.Longitude,
						RecordedDate = x.RecordedDate,
						VehicleCode = x.VehicleCode
					})
					.FirstOrDefaultAsync(cancellationToken);

				if (lastTrackingRecord == null)
				{
					throw new NotFoundException(nameof(TrackingRecordSnapshot.VehicleId), vehicleId);
				}

				lastTrackingRecord.LocalityName = await _geocodingService.ReverseGeocoding(lastTrackingRecord.Latitude, lastTrackingRecord.Longitude, request.APIKey);

				return lastTrackingRecord;
			}
		}
	}
}
