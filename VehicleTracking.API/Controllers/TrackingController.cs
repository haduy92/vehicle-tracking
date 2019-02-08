using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using VehicleTracking.Application.Modules.Commands;
using VehicleTracking.Application.Modules.Models;
using VehicleTracking.Application.Modules.Queries;

namespace VehicleTracking.API.Controllers
{
	[Authorize]
	public class TrackingController : BaseController
	{
		// GET api/tracking/get
		[HttpGet]
		public async Task<ActionResult<VehicleTrackingRecordViewModel>> Get([FromBody] GetTrackingRecordByVehicleQuery query)
		{
			query = query ?? new GetTrackingRecordByVehicleQuery();
			query.Token = RequestToken;
			query.APIKey = APIKey;
			return Ok(await Mediator.Send(query));
		}

		// GET api/tracking/getlist
		[HttpGet]
		public async Task<ActionResult<VehicleTrackingRecordListViewModel>> GetList([FromBody] GetTrackingRecordListByVehicleQuery query)
		{
			query = query ?? new GetTrackingRecordListByVehicleQuery();
			query.Token = RequestToken;
			query.APIKey = APIKey;
			return Ok(await Mediator.Send(query));
		}

		// POST: api/tracking/create
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<IActionResult> Create([FromBody]CreateTrackingRecordCommand command)
		{
			command = command ?? new CreateTrackingRecordCommand();
			command.Token = RequestToken;
			await Mediator.Send(command);
			return NoContent();
		}
	}
}
