using FluentValidation;
using VehicleTracking.Application.Modules.Commands;

namespace VehicleTracking.Application.Modules.Validators
{
	public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
	{
		public UpdateUserCommandValidator()
		{
			RuleFor(x => x.Id)
				.NotNull()
				.NotEmpty();
		}
	}
}
