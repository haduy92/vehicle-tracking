using FluentValidation;
using VehicleTracking.Application.Modules.Commands;

namespace VehicleTracking.Application.Modules.Validators
{
	public class CreateTrackingRecordCommandValidator : AbstractValidator<CreateTrackingRecordCommand>
	{
		public CreateTrackingRecordCommandValidator()
		{
			RuleFor(x => x.DeviceCode)
				.NotNull()
				.NotEmpty();
			RuleFor(x => x.VehicleCode)
				.NotNull()
				.NotEmpty();
			RuleFor(x => x.Latitude)
				.NotNull()
				.NotEmpty()
				.Matches(@"^(\+|-)?(?:90(?:(?:\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,6})?))$") // Source: https://stackoverflow.com/questions/3518504/regular-expression-for-matching-latitude-longitude-coordinates
				.WithMessage("Invalid latitude");
			RuleFor(x => x.Longitude)
				.NotNull()
				.NotEmpty()
				.Matches(@"^(\+|-)?(?:180(?:(?:\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,6})?))$") // Source: https://stackoverflow.com/questions/3518504/regular-expression-for-matching-latitude-longitude-coordinates
				.WithMessage("Invalid latitude");
		}
	}
}
