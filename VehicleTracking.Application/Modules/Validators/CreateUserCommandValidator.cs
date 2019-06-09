using FluentValidation;
using VehicleTracking.Application.Modules.Commands;

namespace VehicleTracking.Application.Modules.Validators
{
	public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
	{
		public CreateUserCommandValidator()
		{
			RuleFor(x => x.Email)
				.NotNull()
				.NotEmpty();
			RuleFor(x => x.Password)
				.NotNull()
				.NotEmpty();
			RuleFor(x => x.FirstName)
				.NotNull()
				.NotEmpty();
			RuleFor(x => x.LastName)
				.NotNull()
				.NotEmpty();
		}
	}
}
