using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(au => au.FirstName)
                .HasMaxLength(100);
            
            builder.Property(au => au.LastName)
                .HasMaxLength(100);

            builder.OwnsOne(au => au.Address, a =>
            {
                a.Property(a => a.Province)
                .HasMaxLength(50);

                a.Property(a => a.City)
                    .HasMaxLength(50);

                a.Property(a => a.Line1)
                    .HasMaxLength(50);

                a.Property(a => a.Line2)
                    .HasMaxLength(20);

                a.Property(a => a.ZipCode)
                    .HasMaxLength(10);

                a.WithOwner();
            });
        }
    }
}