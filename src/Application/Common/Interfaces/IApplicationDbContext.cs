using Domain.Entities.Posts;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<EndUser> Adopters { get; }

        DbSet<PostApplication> Applications { get; }

        DbSet<Fosterer> Fosterers { get; }

        DbSet<Member> Members { get; }

        DbSet<Post> Posts { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}