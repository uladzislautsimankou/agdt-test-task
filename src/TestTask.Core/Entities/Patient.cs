using TestTask.Core.Enums;
using TestTask.Core.Interfaces;

namespace TestTask.Core.Entities;

public class Patient : IIdentifiablyEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    //судя по fhir спецификации может быть null или много (0..*)
    public virtual ICollection<HumanName>? HumanNames { get; set; }

    // судя по fhir спецификации может быть null (0..1)
    public Gender? Gender { get; set; }

    // судя по fhir спецификации может быть null (0..1)
    public DateTime? BirthDate { get; set; }

    // судя по fhir спецификации может быть null (0..1)
    public bool? Active { get; set; }
}
