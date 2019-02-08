using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Persistence.Repositories
{
	public interface IUserRepository : IBaseRepository<User>
	{ }

	public class UserRepository : BaseRepository<User>, IUserRepository
	{
		public UserRepository(IDbContext dbContext, IDateTime dateTime)
			: base(dbContext, dateTime) { }
	}
}
