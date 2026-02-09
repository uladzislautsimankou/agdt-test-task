using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TestTask.Core.Entities;
using TestTask.Core.Repositories;
using TestTask.Core.Utils.FhirDate;
using TestTask.Infrastructure.SQL.Contexts;

namespace TestTask.Infrastructure.SQL.Repositories;

internal sealed class PatientRepository(ApplicationDbContext context) : GenericRepository<Patient, Guid>(context), IPatientRepository
{
    public async Task<IEnumerable<Patient>> GetFilteredPatientsByDatesAsync(
        IEnumerable<string> rawQuery,
        Func<IQueryable<Patient>, IOrderedQueryable<Patient>>? orderBy = null,
        string? includeMany = null,
        Expression<Func<Patient, object>>? include = null,
        int? skip = null,
        int? take = null,
        bool withoutTracking = true,
        bool ignoreGlobalFilters = false)
    {
        return await GetQueryable(
                orderBy: orderBy,
                includeMany: includeMany,
                include: include,
                skip: skip,
                take: take,
                withoutTracking: withoutTracking,
                ignoreGlobalFilters: ignoreGlobalFilters)
            .ApplyFhirDateFilter(x => x.BirthDate, rawQuery)
            .ToListAsync();
    }

}
