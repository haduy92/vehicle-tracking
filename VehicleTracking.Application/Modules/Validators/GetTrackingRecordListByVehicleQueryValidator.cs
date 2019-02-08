using FluentValidation;
using VehicleTracking.Application.Modules.Queries;

namespace VehicleTracking.Application.Modules.Validators
{
	public class GetTrackingRecordListByVehicleQueryValidator : AbstractValidator<GetTrackingRecordListByVehicleQuery>
	{
		public GetTrackingRecordListByVehicleQueryValidator()
		{
			RuleFor(x => x.DeviceCode)
				.NotNull()
				.NotEmpty();
			RuleFor(x => x.VehicleCode)
				.NotNull()
				.NotEmpty();
			RuleFor(a => a.FromDate)
                .LessThan(a => a.ToDate)
                .WithMessage("FromDate must be smaller than ToDate");
		}
	}
}
