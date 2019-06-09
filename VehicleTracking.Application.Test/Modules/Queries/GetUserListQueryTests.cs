using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Modules.Models;
using VehicleTracking.Application.Modules.Queries;
using VehicleTracking.Application.Test.Infrastructure;
using Xunit;

namespace VehicleTracking.Application.Test.Modules.Queries
{
	public class GetUserListQueryTests : QueryTestBase
	{
		[Fact]
		public async Task GetUserList_ShouldReturnAllUsers()
		{
			var sut = new GetUserListQuery.Handler(_context);

			var result = await sut.Handle(new GetUserListQuery(), CancellationToken.None);

			result.ShouldBeOfType<UserListViewModel>();
			result.Users.Count.ShouldBe(3);
		}
	}
}
