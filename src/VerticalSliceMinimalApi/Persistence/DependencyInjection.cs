using Microsoft.EntityFrameworkCore;
using VerticalSliceMinimalApi.Domain.Account;
using VerticalSliceMinimalApi.Persistence.EntityFramework;

namespace VerticalSliceMinimalApi.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("accountdb"));

        services.AddDbContext<ApplicationDbContext>();

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IAccountRepository, AccountRepository>();

        return services;
    }
}