using FluentValidation;
using VehicleTracking.Application.Modules.Queries;

namespace VehicleTracking.Application.Modules.Validators
{
	public class GetTokenQueryValidator : AbstractValidator<GetTokenQuery>
	{
		public GetTokenQueryValidator()
		{
			RuleFor(x => x.EmailAddress)
				.NotNull()
				.NotEmpty();
			RuleFor(x => x.Password)
				.NotNull()
				.NotEmpty();
		}
	}
}
