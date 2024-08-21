using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using VerticalSliceMinimalApi.Domain;
using VerticalSliceMinimalApi.Entities.Account;
using VerticalSliceMinimalApi.Persistence.EntityFramework.Configurations;

namespace VerticalSliceMinimalApi.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;

    public ApplicationDbContext(DbContextOptions options, IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.ApplyConfiguration(new AccountEntityTypeConfiguration());

        base.OnModelCreating(builder);
    }

    public DbSet<Account> Accounts { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.GetDomainEvents().Any())
            .SelectMany(e => e.GetDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return result;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            base.OnConfiguring(optionsBuilder);
        }
        optionsBuilder.EnableSensitiveDataLogging();
    }
}
