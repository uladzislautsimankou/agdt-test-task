using TestTask.ConsoleApp.Clients;
using TestTask.ConsoleApp.Services;

namespace TestTask.ConsoleApp;

internal sealed class App(PatientsApiClient apiClient, TestDataGenerator generator, ILogger<App> logger)
{
    public async Task RunAsync()
    { 
        var fakePatients = generator.Generate(100);

        var result = await apiClient.PostPatientsAsync(fakePatients);

        logger.LogInformation("Отправлено 100 пациентов. Ответ API: {Result}", result);
    }
}
