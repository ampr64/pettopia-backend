using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities.Posts;
using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class PettopiaDbContext : IdentityDbContext<CustomIdentityUser>, IApplicationDbContext
    {
        public static readonly string ConnectionStringKey = "PettopiaDb";
        private readonly IMediator _mediator;

        public PettopiaDbContext(DbContextOptions<PettopiaDbContext> options,
            IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<EndUser> Adopters => Set<EndUser>();

        public DbSet<PostApplication> Applications => Set<PostApplication>();

        public DbSet<BackOfficeUser> BackOfficeUsers => Set<BackOfficeUser>();

        public DbSet<Fosterer> Fosterers => Set<Fosterer>();

        public DbSet<Member> Members => Set<Member>();

        public DbSet<Post> Posts => Set<Post>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ConfigureSmartEnum();
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEventEntities = ChangeTracker
                .Entries<IHasDomainEvents>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            foreach (var entity in domainEventEntities)
            {
                var domainEvents = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();

                foreach (var domainEvent in domainEvents)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}