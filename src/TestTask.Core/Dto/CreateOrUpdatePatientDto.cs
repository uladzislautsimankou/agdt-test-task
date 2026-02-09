using TestTask.Core.Enums;

namespace TestTask.Core.Dto;

public record CreateOrUpdatePatientDto(Gender? Gender, DateTime? BirthDate, bool? Active, IEnumerable<CreateOrUpdateHumanNameDto> Name);

public record CreateOrUpdateHumanNameDto(NameUse? Use, string? Family, IEnumerable<string> Given);
