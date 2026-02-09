using TestTask.ConsoleApp;
using TestTask.ConsoleApp.Clients;
using TestTask.ConsoleApp.Services;
using TestTask.ConsoleApp.Settings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("Api"));
builder.Services.AddHttpClient<PatientsApiClient>((sp, client) =>
{
    var settings = sp.GetRequiredService<IConfiguration>().GetSection("Api").Get<ApiSettings>();
    client.BaseAddress = new Uri(settings.BaseUrl);
});

builder.Services.AddSingleton<TestDataGenerator>();
builder.Services.AddTransient<App>();

var host = builder.Build();

using var scope = host.Services.CreateScope();
var app = scope.ServiceProvider.GetRequiredService<App>();
await app.RunAsync();
Console.ReadKey();