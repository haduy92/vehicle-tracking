using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence;

namespace VehicleTracking.Application.Modules.Commands
{
	public class CreateVehicleCommand : BaseRequest, IRequest
	{
		public string VehicleCode { get; set; }
		public string DeviceCode { get; set; }

		// Extended property
		public string Fuel { get; set; }
		// Extended property
		public string Speed { get; set; }

		public class Handler : IRequestHandler<CreateVehicleCommand>
		{
			private readonly VehicleTrackingDbContext _context;
			private readonly IDateTime _dateTime;
			private readonly IMediator _mediator;

			public Handler(VehicleTrackingDbContext context, IDateTime dateTime, IMediator mediator)
			{
				_context = context;
				_dateTime = dateTime;
				_mediator = mediator;
			}

			public async Task<Unit> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
			{
				// Check if vehicle and device code exist
				var exist = _context.Vehicles
					.AsNoTracking()
					.AnyAsync(x => x.VehicleCode.ToLower() == request.VehicleCode.ToLower()
						&& x.DeviceCode.ToLower() == request.DeviceCode.ToLower()
						&& x.IsActive, cancellationToken);

				if (await exist)
				{
					throw new DuplicatedException(nameof(Vehicle), request.VehicleCode);
				}

				var dnr = new Dictionary<string, string>();
				dnr.Add(nameof(Fuel), request.Fuel);
				dnr.Add(nameof(Speed), request.Speed);

				var vehile = new Vehicle
				{
					DeviceCode = request.DeviceCode,
					VehicleCode = request.VehicleCode,
					ExtendedData = ToXML(dnr)
				};

				_context.Vehicles.Add(vehile);
				await _context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}

			private string ToXML(Dictionary<string, string> dnr)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("<ExtendedData>");

				foreach (var entry in dnr)
				{
					sb.Append("<" + entry.Key + "><![CDATA[" + entry.Value + "]]></" + entry.Key + ">");
				}

				sb.Append("</ExtendedData>");

				return sb.ToString();
			}
		}
	}
}
