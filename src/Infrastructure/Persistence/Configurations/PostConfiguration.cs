using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.OwnsMany(p => p.Images);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(p => p.CreatedBy);
        }
    }
}