using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence.Configurations
{
	internal class TrackingRecordSnapshotConfiguration : IEntityTypeConfiguration<TrackingRecordSnapshot>
	{
		public void Configure(EntityTypeBuilder<TrackingRecordSnapshot> builder)
		{
			builder.ToTable("TrackingRecordSnapshots");
		}
	}
}
