using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Core.Entities;

namespace TestTask.Infrastructure.SQL.Configurations;

internal sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder
            .HasMany(p => p.HumanNames)
            .WithOne()
            .HasForeignKey(h => h.PatientId);
    }
}
