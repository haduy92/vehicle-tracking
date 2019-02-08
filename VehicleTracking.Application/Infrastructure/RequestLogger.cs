using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Modules.Queries;
using VehicleTracking.Common;

namespace VehicleTracking.Application.Infrastructure
{
	public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
		where TRequest : BaseRequest
	{
		private readonly ILogger _logger;
		private readonly IToken _token;

		public RequestLogger(ILogger<TRequest> logger, IToken token)
		{
			_logger = logger;
			_token = token;
		}

		public Task Process(TRequest request, CancellationToken cancellationToken)
		{
			var name = typeof(TRequest).Name;

			if (request is GetTokenQuery)
			{
				var tokenQuery = request as GetTokenQuery;
				_logger.LogInformation("Request: {@Name} by email {@Email}", name, tokenQuery.EmailAddress);
			}
			else
			{
				// Add User Details
				var userId = _token.GetValue(request.Token, "user_id");

				_logger.LogInformation("Request: {@Name} by user {@UserId}", name, userId);
			}

			return Task.CompletedTask;
		}
	}
}
