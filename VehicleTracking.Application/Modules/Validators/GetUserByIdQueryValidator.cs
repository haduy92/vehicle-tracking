using FluentValidation;
using VehicleTracking.Application.Modules.Queries;

namespace VehicleTracking.Application.Modules.Validators
{
	public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
	{
		public GetUserByIdQueryValidator()
		{
			RuleFor(x => x.Id)
				.NotNull()
				.NotEmpty();
		}
	}
}
