using Microsoft.Extensions.DependencyInjection;
using TestTask.Core.Services;
using TestTask.Core.Services.Interfaces;

namespace TestTask.Core;

public static class ServicesRegistration
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddScoped<IPatientsService, PatientsService>();
    }
}