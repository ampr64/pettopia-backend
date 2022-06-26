using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities.Posts;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class PettopiaDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public static readonly string ConnectionStringKey = "PettopiaDb";
        private readonly IMediator _mediator;

        public PettopiaDbContext(DbContextOptions<PettopiaDbContext> options,
            IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<PostApplication> Applications => Set<PostApplication>();

        public DbSet<Post> Posts => Set<Post>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.ConfigureSmartEnum();

            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
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
                    _mediator.Publish(domainEvent, cancellationToken);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}