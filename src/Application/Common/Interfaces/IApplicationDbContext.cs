using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Post> Posts { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}