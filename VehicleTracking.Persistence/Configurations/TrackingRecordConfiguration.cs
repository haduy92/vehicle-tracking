using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence.Configurations
{
	internal class TrackingRecordConfiguration : IEntityTypeConfiguration<TrackingRecord>
	{
		public void Configure(EntityTypeBuilder<TrackingRecord> builder)
		{
			builder.ToTable("TrackingRecords");
			builder.Property(c => c.DeviceCode).HasMaxLength(50);
			builder.Property(c => c.VehicleCode).HasMaxLength(50);
			builder.Property(c => c.Latitude).HasMaxLength(50);
			builder.Property(c => c.Longitude).HasMaxLength(50);
			// Note: Cluster the table on this index in order to speed up queries
			builder.HasIndex(c => c.TrackingRecordSnapshotId);
		}
	}
}
