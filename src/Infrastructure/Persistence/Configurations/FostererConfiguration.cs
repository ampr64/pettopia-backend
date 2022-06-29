using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class FostererConfiguration : IEntityTypeConfiguration<Fosterer>
    {
        public void Configure(EntityTypeBuilder<Fosterer> builder)
        {
            builder.Property(f => f.OrganizationName)
                .HasMaxLength(100);

            builder.OwnsOne(f => f.ApplicationForm, a =>
            {
                a.WithOwner()
                    .HasForeignKey(a => a.FostererId);

                a.OwnsMany(a => a.Requirements, r =>
                {
                    r.WithOwner();

                    r.Property(r => r.Requirement)
                        .HasMaxLength(200);
                });
            });

            builder.OwnsMany(f => f.Pictures, p =>
            {
                p.WithOwner()
                    .HasForeignKey(p => p.FostererId);
            });
        }
    }
}