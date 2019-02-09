using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VehicleTracking.Application.Modules.Commands;
using VehicleTracking.Application.Modules.Models;
using VehicleTracking.Application.Modules.Queries;

namespace VehicleTracking.API.Controllers
{
	[Authorize]
	public class UsersController : BaseController
	{
		// GET: api/users/get/[id]
		[HttpGet("{id}")]
		public async Task<ActionResult<UserViewModel>> Get(string id)
		{
			return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
		}

		// GET: api/users/getlist/
		[HttpGet("{id}")]
		public async Task<ActionResult<UserListViewModel>> GetList([FromBody]GetUserListQuery query)
		{
			return Ok(await Mediator.Send(query));
		}

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
		public async Task<ActionResult<string>> Create([FromBody]CreateUserCommand command)
		{
			return Ok(await Mediator.Send(command));
		}

		// PUT: api/users/update/[id]
		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserCommand command)
		{
			await Mediator.Send(command);

			return NoContent();
		}

		// DELETE: api/users/[id]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			await Mediator.Send(new DeleteUserCommand { Id = id });

			return NoContent();
		}
	}
}