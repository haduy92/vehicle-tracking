using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Modules.Notifications.Models;

namespace VehicleTracking.Application.Modules.Notifications
{
	public class UserCreated : INotification
	{
		public string Id { get; set; }
		public string Email { get; set; }

		public class UserCreatedHandler : INotificationHandler<UserCreated>
		{
			private readonly INotificationService _notification;

			public UserCreatedHandler(INotificationService notification)
			{
				_notification = notification;
			}

			public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
            {
				// TODO: OnUserCreated
                await _notification.SendAsync(new Message());
				// TODO: OnAfterUserCreated
            }
		}
	}
}
