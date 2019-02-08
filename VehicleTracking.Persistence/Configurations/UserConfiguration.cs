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

			builder.OwnsOne(p => p.Address).Property(p => p.City).HasMaxLength(60).HasColumnName("City");
			builder.OwnsOne(p => p.Address).Property(p => p.Country).HasMaxLength(60).HasColumnName("Country");
			builder.OwnsOne(p => p.Address).Property(p => p.State).HasMaxLength(60).HasColumnName("State");
			builder.OwnsOne(p => p.Address).Property(p => p.StreetAddress1).HasMaxLength(60).HasColumnName("StreetAddress1");
			builder.OwnsOne(p => p.Address).Property(p => p.StreetAddress2).HasMaxLength(60).HasColumnName("StreetAddress2");
			builder.OwnsOne(p => p.Address).Property(p => p.StreetAddress3).HasMaxLength(60).HasColumnName("StreetAddress3");
			builder.OwnsOne(p => p.Address).Property(p => p.ZipCode).HasMaxLength(10).HasColumnName("ZipCode");
		}
	}
}
