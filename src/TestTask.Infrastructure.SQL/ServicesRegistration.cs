using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestTask.Core.Repositories;
using TestTask.Core.UnitOfWork;
using TestTask.Infrastructure.SQL.Contexts;
using TestTask.Infrastructure.SQL.Repositories;
using TestTask.Infrastructure.SQL.UnitOfWork;

namespace TestTask.Infrastructure.SQL;

public static class ServicesRegistration
{
    public static void AddInfrastructureSQL(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlServer(
               configuration.GetConnectionString("ApplicationDatabaseConnection"),
               b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IHumanNameRepository, HumanNameRepository>();
        services.AddScoped<IGivenNameRepository, GivenNameRepository>();

        services.AddScoped<IUnitOfWork, EFUnitOfWork>();
    }
}
