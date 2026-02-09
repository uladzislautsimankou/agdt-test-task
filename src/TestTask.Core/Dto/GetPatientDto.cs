using TestTask.Core.Entities;
using TestTask.Core.Enums;

namespace TestTask.Core.Dto;

public record GetPatientDto
{
    public GetPatientDto(Patient patient)
    {
        Id = patient.Id;
        Gender = patient.Gender;
        BirthDate = patient.BirthDate;
        Active = patient.Active;
        Name = patient.HumanNames?.Select(x => new GetHumanNameDto(x)).ToList();
    }

    public Guid Id { get; set; }

    public IEnumerable<GetHumanNameDto>? Name {  get; set; }

    public Gender? Gender { get; set; }

    public DateTime? BirthDate { get; set; }

    public bool? Active { get; set; }
}

public record GetHumanNameDto
{
    public GetHumanNameDto(HumanName humanName)
    {
        Id = humanName.Id;
        Use = humanName.Use;
        Family = humanName.Family;
        Given = humanName.GivenNames?.Select(x => x.Name).ToList();
    }

    public Guid Id { get; set; }

    public NameUse? Use { get; set; }

    public string? Family { get; set; }

    public IEnumerable<string>? Given { get; set; }
}