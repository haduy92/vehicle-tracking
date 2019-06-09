using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Persistence.Configurations
{
	internal class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");
			builder.Property(p => p.PasswordHash).HasMaxLength(64);
			builder.Property(p => p.PasswordSalt).HasMaxLength(128);
			builder.Property(p => p.FirstName).HasMaxLength(50);
			builder.Property(p => p.LastName).HasMaxLength(50);

			builder.OwnsOne(p => p.Email);
			builder.OwnsOne(p => p.Address);
		}
	}
}
