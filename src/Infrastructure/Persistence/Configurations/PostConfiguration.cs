using Domain.Entities.Posts;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasOne<Member>()
                .WithMany(m => m.Posts)
                .HasForeignKey(p => p.CreatedBy);

            builder.OwnsMany(p => p.Images);

            builder.HasMany(p => p.Applications)
                .WithOne()
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}