using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VehicleTracking.Application.Modules.Commands;

namespace VehicleTracking.API.Controllers
{
	[Authorize]
    public class VehiclesController : BaseController
    {
        // POST: api/vehicles/create
		[HttpPost]
		public async Task<ActionResult<string>> Create([FromBody]CreateVehicleCommand command)
		{
			command = command ?? new CreateVehicleCommand();
			command.Token = RequestToken;
			await Mediator.Send(command);
			return NoContent();
		}
    }
}