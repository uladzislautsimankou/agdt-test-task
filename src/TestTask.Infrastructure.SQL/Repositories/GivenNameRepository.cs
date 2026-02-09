using TestTask.Core.Entities;
using TestTask.Core.Repositories;
using TestTask.Infrastructure.SQL.Contexts;

namespace TestTask.Infrastructure.SQL.Repositories;

internal sealed class GivenNameRepository(ApplicationDbContext context) : GenericRepository<GivenName, Guid>(context), IGivenNameRepository
{
}
