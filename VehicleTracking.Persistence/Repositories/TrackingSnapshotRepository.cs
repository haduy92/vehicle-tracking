using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.Persistence.Repositories
{
	public interface ITrackingSnapshotRepository : IBaseRepository<TrackingRecordSnapshot>
	{ }

	public class TrackingSnapshotRepository : BaseRepository<TrackingRecordSnapshot>, ITrackingSnapshotRepository
	{
		public TrackingSnapshotRepository(IDbContext dbContext, IDateTime dateTime)
			: base(dbContext, dateTime) { }
	}
}
