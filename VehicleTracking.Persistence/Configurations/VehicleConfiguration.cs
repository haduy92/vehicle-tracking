using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence.Configurations
{
	internal class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
	{
		public void Configure(EntityTypeBuilder<Vehicle> builder)
		{
			builder.ToTable("Vehicles");
			builder.Property(p => p.DeviceCode).HasMaxLength(50);
			builder.Property(p => p.VehicleCode).HasMaxLength(50);
			builder.HasIndex(p => p.DeviceCode).IsUnique();
			builder.HasIndex(p => p.VehicleCode).IsUnique();
		}
	}
}
