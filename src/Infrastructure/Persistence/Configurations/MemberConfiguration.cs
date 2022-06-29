using Domain.Entities.Users;
using Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasDiscriminator(m => m.Role)
                .HasValue<EndUser>(Role.User)
                .HasValue<Fosterer>(Role.Fosterer)
                .HasValue<BackOfficeUser>(Role.BackOfficeUser)
                .HasValue<Administrator>(Role.Admin);

            builder.Property(m => m.Role)
                .HasConversion(
                    r => r.Name,
                    r => Role.FromName(r, false)
                );

            builder.Property(m => m.FirstName)
                .HasMaxLength(100);

            builder.Property(m => m.LastName)
                .HasMaxLength(100);

            builder.Property(m => m.Email)
                    .HasMaxLength(100);

            builder.Property(m => m.InstagramProfileUrl)
                .HasMaxLength(100);

            builder.Property(m => m.FacebookProfileUrl)
                .HasMaxLength(100);

            builder.OwnsOne(m => m.PhoneNumber, p =>
            {
                p.Property(p => p.Prefix)
                    .HasMaxLength(5);

                p.Property(p => p.Number)
                    .HasMaxLength(15);
            });

            builder.OwnsOne(m => m.Address, a =>
            {
                a.WithOwner();

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
            });

            builder.OwnsOne(m => m.ProfilePicture);

            builder.HasQueryFilter(m => m.Status == AccountStatus.Active);
        }
    }
}