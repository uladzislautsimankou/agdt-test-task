using System.Linq.Expressions;
using TestTask.Core.Entities;

namespace TestTask.Core.Repositories;

public interface IPatientRepository : IGenericRepository<Patient, Guid>
{
    Task<IEnumerable<Patient>> GetFilteredPatientsByDatesAsync(
        IEnumerable<string> rawQuery,
        Func<IQueryable<Patient>, IOrderedQueryable<Patient>>? orderBy = null,
        string? includeMany = null,
        Expression<Func<Patient, object>>? include = null,
        int? skip = null,
        int? take = null,
        bool withoutTracking = true,
        bool ignoreGlobalFilters = false);
}
