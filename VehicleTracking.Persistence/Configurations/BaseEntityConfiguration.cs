using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence.Configurations
{
	internal abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
		where T : BaseEntity
	{
		public virtual void Configure(EntityTypeBuilder<T> builder)
		{ }
	}
}
