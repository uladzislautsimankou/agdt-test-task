using TestTask.Core.Enums;
using TestTask.Core.Interfaces;

namespace TestTask.Core.Entities;

public class HumanName : IIdentifiablyEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PatientId { get; set; }

    // судя по fhir спецификации может быть null (0..1)
    public NameUse? Use { get; set; }

    // судя по fhir спецификации может быть null (0..1)
    public string? Family {  get; set; }

    // по идее, должно быть просто стрингой, но, как будто, это нормальные формы БД нарушает
    public virtual ICollection<GivenName>? GivenNames { get; set; }
}
