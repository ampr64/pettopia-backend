using Domain.Entities.Posts;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PostApplicationConfiguration : IEntityTypeConfiguration<PostApplication>
    {
        public void Configure(EntityTypeBuilder<PostApplication> builder)
        {
            builder.Property(pa => pa.Id)
                .ValueGeneratedNever();

            builder.HasOne<Member>()
                .WithMany()
                .HasForeignKey(a => a.ApplicantId);

            builder.OwnsOne(pa => pa.ApplicantInfo, ai =>
            {
                ai.WithOwner()
                    .HasForeignKey(a => a.ApplicationId);

                ai.Property(a => a.Email)
                    .HasMaxLength(100);

                ai.Property(a => a.Name)
                    .HasMaxLength(100);

                ai.OwnsOne(a => a.PhoneNumber, p =>
                {
                    p.WithOwner();

                    p.Property(p => p.Prefix)
                        .HasMaxLength(5);

                    p.Property(p => p.Number)
                        .HasMaxLength(12);
                });
            });
        }
    }
}