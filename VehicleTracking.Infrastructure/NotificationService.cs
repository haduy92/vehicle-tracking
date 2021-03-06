﻿using System.Threading.Tasks;
using VehicleTracking.Application.Interfaces;
using VehicleTracking.Application.Modules.Notifications.Models;

namespace VehicleTracking.Infrastructure
{
	public class NotificationService : INotificationService
    {
        public Task SendAsync(Message message)
        {
			// Send email with message
            return Task.CompletedTask;
        }
    }
}
