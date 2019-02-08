using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Persistence.Infrastructure;


namespace VehicleTracking.Persistence.Repositories
{
	public interface IAuditLogRepository : IBaseRepository<AuditLog>
	{ }

	public class AuditLogRepository : BaseRepository<AuditLog>, IAuditLogRepository
	{
		public AuditLogRepository(IDbContext dbContext, IDateTime dateTime)
			: base(dbContext, dateTime) { }
	}
}
