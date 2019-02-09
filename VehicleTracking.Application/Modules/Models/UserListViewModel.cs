using System.Collections.Generic;

namespace VehicleTracking.Application.Modules.Models
{
	public class UserListViewModel
	{
		public IEnumerable<UserViewModel> Users { get; set; }
		public bool CreateEnabled { get; set; }
	}
}
