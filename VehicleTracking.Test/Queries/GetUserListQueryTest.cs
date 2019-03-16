using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Application.Modules.Queries;
using VehicleTracking.Domain.Entities;
using Xunit;

namespace VehicleTracking.Test.Queries
{
	[Collection("QueryCollection")]
	public class GetUserListQueryTest : TestBase
	{
		[Fact]
		public async Task GetUserListTest()
		{
			_context.UserRepository.CreateMany(new[] 
			{
				new User { FirstName = "User", LastName = "01" },
				new User { FirstName = "User", LastName = "02" },
				new User { FirstName = "User", LastName = "03" }
			});

			_context.Commit();

			var query = new GetUserListQuery.Handler(_context);

			var result = await query.Handle(new GetUserListQuery(), CancellationToken.None);

			Assert.Equal(3, result.Users.Count());
		}
	}
}
