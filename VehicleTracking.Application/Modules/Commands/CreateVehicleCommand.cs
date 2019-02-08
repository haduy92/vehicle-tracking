using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Exceptions;
using VehicleTracking.Application.Infrastructure;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;

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
			private readonly IUnitOfWork _unitOfWork;
			private readonly IMediator _mediator;

			public Handler(IUnitOfWork unitOfWork, IMediator mediator)
			{
				_unitOfWork = unitOfWork;
				_mediator = mediator;
			}

			public async Task<Unit> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
			{
				// Check if vehicle and device code exist
				var exist = _unitOfWork.VehicleRepository
					.GetQueryableAsNoTracking(x => x.VehicleCode.ToLower() == request.VehicleCode.ToLower()
						&& x.DeviceCode.ToLower() == request.DeviceCode.ToLower()
						&& x.IsActive)
					.Select(x => x.Id)
					.AnyAsync(cancellationToken);

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

				_unitOfWork.VehicleRepository.Create(vehile);
				_unitOfWork.Commit();

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
