using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestTask.Core.Entities;

namespace TestTask.Infrastructure.SQL.Configurations;

internal sealed class HumanNameConfiguration : IEntityTypeConfiguration<HumanName>
{
    public void Configure(EntityTypeBuilder<HumanName> builder)
    {
        builder
            .HasMany(h => h.GivenNames)
            .WithOne()
            .HasForeignKey(g => g.HumanNameId);
    }
}
