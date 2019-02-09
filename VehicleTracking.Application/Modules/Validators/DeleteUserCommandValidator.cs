using FluentValidation;
using VehicleTracking.Application.Modules.Commands;

namespace VehicleTracking.Application.Modules.Validators
{
	public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
	{
		public DeleteUserCommandValidator()
		{
			RuleFor(x => x.Id)
				.NotNull()
				.NotEmpty();
		}
	}
}
