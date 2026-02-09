using System.Net.Http.Json;
using TestTask.Core.Dto;

namespace TestTask.ConsoleApp.Clients;

internal sealed class PatientsApiClient(HttpClient http)
{
    public async Task<string> PostPatientsAsync(IEnumerable<CreateOrUpdatePatientDto> patients)
    {
        var response = await http.PostAsJsonAsync("Patients/batch", patients);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
