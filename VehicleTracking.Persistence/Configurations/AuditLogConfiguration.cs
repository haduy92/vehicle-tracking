using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.Enumerations;

namespace VehicleTracking.Persistence.Configurations
{
	internal class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
	{
		public void Configure(EntityTypeBuilder<AuditLog> builder)
		{
			builder.ToTable("AuditLogs");
			builder.Property(c => c.TableName).HasMaxLength(50);
			builder.Property(c => c.Action).HasMaxLength(10);
			builder.Property(c => c.Action).HasConversion(new EnumToStringConverter<AuditAction>());
		}
	}
}
