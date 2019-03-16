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
using VehicleTracking.Persistence;
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
			private readonly VehicleTrackingDbContext _context;
			private readonly IGeocodingService _geocodingService;

			public Handler(VehicleTrackingDbContext context, IGeocodingService geocodingService)
			{
				_context = context;
				_geocodingService = geocodingService;
			}

			public async Task<VehicleTrackingRecordViewModel> Handle(GetTrackingRecordByVehicleQuery request, CancellationToken cancellationToken)
			{
				// Check if input vehicle and device code exist
				Guid vehicleId = _context.Vehicles
					.AsNoTracking()
					.Where(x => x.VehicleCode.ToLower() == request.VehicleCode.ToLower()
						&& x.DeviceCode.ToLower() == request.DeviceCode.ToLower()
						&& x.IsActive)
					.Select(x => x.Id)
					.SingleOrDefault();

				if (vehicleId == null || vehicleId == Guid.Empty)
				{
					throw new NotFoundException(nameof(Vehicle), request.VehicleCode);
				}

				// Get last vehicle tracking record
				var lastTrackingRecord = await _context.TrackingRecordSnapshots
					.AsNoTracking()
					.Where(x => x.VehicleId == vehicleId)
					.OrderByDescending(x => x.RecordedDate)
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
