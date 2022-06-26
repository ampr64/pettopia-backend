using Domain.Entities.Posts;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<PostApplication> Applications { get; }

        DbSet<Post> Posts { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}