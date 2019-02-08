using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Persistence.Repositories
{
	public interface ITrackingRepository : IBaseRepository<TrackingRecord>
	{ }

	public class TrackingRepository : BaseRepository<TrackingRecord>, ITrackingRepository
	{
		public TrackingRepository(IDbContext dbContext, IDateTime dateTime)
			: base(dbContext, dateTime) { }
	}
}
