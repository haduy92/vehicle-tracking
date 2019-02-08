using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Persistence.Repositories
{
	public interface IVehicleRepository : IBaseRepository<Vehicle>
	{ }

	public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
	{
		public VehicleRepository(IDbContext dbContext, IDateTime dateTime)
			: base(dbContext, dateTime) { }
	}
}
