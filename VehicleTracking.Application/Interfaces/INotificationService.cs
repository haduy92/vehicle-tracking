using System.Threading.Tasks;
using VehicleTracking.Application.Modules.Notifications.Models;

namespace VehicleTracking.Application.Interfaces
{
	public interface INotificationService
    {
        Task SendAsync(Message message);
    }
}
