using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VehicleTracking.Application.Modules.Commands;
using VehicleTracking.Application.Modules.Queries;

namespace VehicleTracking.API.Controllers
{
	public class UsersController : BaseController
	{
		// POST: api/users/login
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody]GetTokenQuery query)
		{
			query = query ?? new GetTokenQuery();
			query.SecretKey = SecretKey;
			query.Issuer = Issuer;
			return Ok(new { token = await Mediator.Send(query) });
		}

		// POST: api/users/create
		[HttpPost]
		[Authorize]
		public async Task<ActionResult<string>> Create([FromBody]CreateUserCommand command)
		{
			return Ok(await Mediator.Send(command));
		}
	}
}