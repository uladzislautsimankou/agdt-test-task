using TestTask.Core.Dto;

namespace TestTask.Core.Services.Interfaces;

public interface IPatientsService
{
    Task<Guid> CreatePatientAsync(CreateOrUpdatePatientDto patient);

    Task<IReadOnlyList<Guid>> CreatePatientsAsync(IEnumerable<CreateOrUpdatePatientDto> patients);

    Task UpdatePatientAsync(Guid id, CreateOrUpdatePatientDto patient);

    Task<PatchPatientDto> GetPatchPatientDtoAsync(Guid id);

    Task PatchPatientAsync(Guid id, PatchPatientDto patient);

    Task<GetPatientDto> GetPatientAsync(Guid id);

    Task<IReadOnlyList<GetPatientDto>> GetAllPatientsIncludeNamesAsync();

    Task<IReadOnlyList<GetPatientDto>> SearchPatientsByBirthDateAsync(List<string> rawDates);

    Task DeletePatientAsync(Guid id);
}
