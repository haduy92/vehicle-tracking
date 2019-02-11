using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Helpers;
using VehicleTracking.Application.Modules.Queries;

namespace VehicleTracking.Application.Infrastructure
{
	public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
		where TRequest : BaseRequest
	{
		private readonly ILogger _logger;

		public RequestLogger(ILogger<TRequest> logger)
		{
			_logger = logger;
		}

		public Task Process(TRequest request, CancellationToken cancellationToken)
		{
			var userInfo = "";

			if (request is GetTokenQuery tokenQuery)
			{
				userInfo = $"email {tokenQuery.EmailAddress}";
			}
			else
			{
				userInfo = $"user {TokenHelper.GetValue(request.Token, "user_id")}";
			}

			_logger.LogInformation("Request by {UserInfo}", userInfo);

			return Task.CompletedTask;
		}
	}
}
