using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PW.Elearning.Domain.Repositories;
using PW.Elearning.Infrastructure.Persistence;
using PW.Elearning.Infrastructure.Repositories;

namespace PW.Elearning.Infrastructure;

public static class DependencyInjection
{
    private static readonly ServerVersion MySqlVersion = new MySqlServerVersion(new Version(8, 0, 36));

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ElearningDbContext>(options =>
            options.UseMySql(connectionString, MySqlVersion));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ElearningDbContext>());
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICourseRepository, CourseRepository>();

        return services;
    }
}
