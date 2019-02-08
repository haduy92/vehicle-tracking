using FluentValidation;
using VehicleTracking.Application.Modules.Queries;

namespace VehicleTracking.Application.Modules.Validators
{
	public class GetTrackingRecordByVehicleQueryValidator : AbstractValidator<GetTrackingRecordByVehicleQuery>
	{
		public GetTrackingRecordByVehicleQueryValidator()
		{
			RuleFor(x => x.DeviceCode)
				.NotNull()
				.NotEmpty();
			RuleFor(x => x.VehicleCode)
				.NotNull()
				.NotEmpty();
		}
	}
}
