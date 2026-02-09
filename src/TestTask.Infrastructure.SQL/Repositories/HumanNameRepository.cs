using TestTask.Core.Entities;
using TestTask.Core.Repositories;
using TestTask.Infrastructure.SQL.Contexts;

namespace TestTask.Infrastructure.SQL.Repositories;

internal sealed class HumanNameRepository(ApplicationDbContext context) : GenericRepository<HumanName, Guid>(context), IHumanNameRepository
{
}
