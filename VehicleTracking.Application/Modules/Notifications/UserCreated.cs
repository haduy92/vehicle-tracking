using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Modules.Notifications.Models;

namespace VehicleTracking.Application.Modules.Notifications
{
	public class UserCreated : INotification
	{
		public string UserId { get; set; }

		public class UserCreatedHandler : INotificationHandler<UserCreated>
		{
			private readonly INotificationService _notification;

			public UserCreatedHandler(INotificationService notification)
			{
				_notification = notification;
			}

			public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
            {
                await _notification.SendAsync(new Message());
            }
		}
	}
}
