using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VehicleTracking.API.Controllers
{
	[ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : Controller
    {
        private IMediator _mediator;
		private IConfiguration _configuration;

		protected IConfiguration Configuration => _configuration ?? (_configuration = HttpContext.RequestServices.GetService<IConfiguration>());
		protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

		protected string SecretKey => Configuration.GetSection("Jwt:SecretKey")?.Value;
		protected string Issuer => Configuration.GetSection("Jwt:Issuer")?.Value;
		protected string APIKey => Configuration.GetSection("GoogleAPI:Key")?.Value;
		protected string RequestToken => HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    }
}