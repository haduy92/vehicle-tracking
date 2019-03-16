using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Modules.Models;
using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence;

namespace VehicleTracking.Application.Modules.Queries
{
	public class GetTrackingRecordListByVehicleQuery : BaseRequest, IRequest<VehicleTrackingRecordListViewModel>
	{
		public string VehicleCode { get; set; }
		public string DeviceCode { get; set; }
		public string APIKey { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }

		public class Handler : IRequestHandler<GetTrackingRecordListByVehicleQuery, VehicleTrackingRecordListViewModel>
		{
			private readonly VehicleTrackingDbContext _context;
			private readonly IDateTime _dateTime;
			private readonly IGeocodingService _geocodingService;

			public Handler(VehicleTrackingDbContext context, IGeocodingService geocodingService, IDateTime dateTime)
			{
				_context = context;
				_geocodingService = geocodingService;
				_dateTime = dateTime;
			}

			public async Task<VehicleTrackingRecordListViewModel> Handle(GetTrackingRecordListByVehicleQuery request, CancellationToken cancellationToken)
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

				var fromDate = request.FromDate.ToUniversalTime();
				var toDate = request.ToDate.ToUniversalTime();

				// Get list vehicle tracking records
				var listTrackingRecords = _context.TrackingRecordSnapshots
					.AsNoTracking()
					.Where(x => x.VehicleId == vehicleId 
						&& x.RecordedDate >= fromDate.Date
						&& x.RecordedDate <= toDate.Date)
					.OrderByDescending(x => x.RecordedDate)
					.SelectMany(x => x.TrackingRecords)
					.Where(x => x.RecordedDate >= fromDate && x.RecordedDate <= toDate)
					.OrderByDescending(x => x.RecordedDate)
					.Select(x => new TrackingRecordViewModel
					{
						Latitude = x.Latitude,
						Longitude = x.Longitude,
						RecordedDate = x.RecordedDate,
					})
					.ToListAsync(cancellationToken);

				var model = new VehicleTrackingRecordListViewModel
				{
					VehicleCode = request.VehicleCode,
					TrackingRecords = await listTrackingRecords
				};

				List<Task> listTasks = new List<Task>();
				foreach (var tracking in model.TrackingRecords)
				{
					listTasks.Add(FillLocalityName(tracking, request.APIKey));
				}

				await Task.WhenAll(listTasks);

				return model;
			}

			private async Task FillLocalityName(TrackingRecordViewModel model, string apiKey)
			{
				model.LocalityName = await _geocodingService.ReverseGeocoding(model.Latitude, model.Longitude, apiKey);
			}
		}
	}
}
