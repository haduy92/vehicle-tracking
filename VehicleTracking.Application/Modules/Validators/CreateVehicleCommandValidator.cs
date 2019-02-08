using FluentValidation;
using VehicleTracking.Application.Modules.Commands;

namespace VehicleTracking.Application.Modules.Validators
{
	public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
	{
		public CreateVehicleCommandValidator()
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
