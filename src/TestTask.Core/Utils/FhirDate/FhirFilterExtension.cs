using System.Linq.Expressions;

namespace TestTask.Core.Utils.FhirDate;

public static class FhirFilterExtension
{
    public static IQueryable<T> ApplyFhirDateFilter<T>(
        this IQueryable<T> query,
        Expression<Func<T, DateTime?>> selector,
        IEnumerable<string> rawParams)
    {
        var parameter = selector.Parameters[0];
        Expression? finalExpression = null;

        foreach (var group in rawParams)
        {
            Expression? groupExpression = null;
            var fhirDates = FhirDateParcer.ParseFhirDate(group);

            foreach (var date in fhirDates)
            {
                var bounds = GetBounds(date);

                // строим выражение
                Expression conditionExpression = date.Prefix switch
                {
                    // != убираем диапазон
                    FhirDatePrefix.Ne => Expression.OrElse(
                                Expression.LessThan(selector.Body, Expression.Constant(bounds.lowerBound, typeof(DateTime?))),
                                Expression.GreaterThan(selector.Body, Expression.Constant(bounds.upperBound, typeof(DateTime?)))),
                    // > если указан только 24 год, то начинаем с 25
                    FhirDatePrefix.Gt => Expression.GreaterThan(selector.Body, Expression.Constant(bounds.upperBound, typeof(DateTime?))),
                    // >= если указан только 24 год, то с него можно и начать
                    FhirDatePrefix.Ge => Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(bounds.lowerBound, typeof(DateTime?))),
                    // < если указан только 24, то начинаем с 23
                    FhirDatePrefix.Lt => Expression.LessThan(selector.Body, Expression.Constant(bounds.lowerBound, typeof(DateTime?))),
                    // <= если указан только 24, то с него и начинаем
                    FhirDatePrefix.Le => Expression.LessThan(selector.Body, Expression.Constant(bounds.upperBound, typeof(DateTime?))),
                    //если приблизительно, то просто входит в диапазон включительн
                    FhirDatePrefix.Ap => Expression.AndAlso(
                                Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(bounds.lowerBound, typeof(DateTime?))),
                                Expression.LessThanOrEqual(selector.Body, Expression.Constant(bounds.upperBound, typeof(DateTime?)))),

                    // eq == значит входи в диапазон
                    _ => Expression.AndAlso(
                                Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(bounds.lowerBound, typeof(DateTime?))),
                                Expression.LessThanOrEqual(selector.Body, Expression.Constant(bounds.upperBound, typeof(DateTime?))))
                };

                //внутри группы ||
                groupExpression = groupExpression is null
                    ? conditionExpression
                    : Expression.OrElse(groupExpression, conditionExpression);
            }
            //группы склеиваются &&
            if (groupExpression is not null)
                finalExpression = finalExpression is null
                    ? groupExpression
                    : Expression.AndAlso(finalExpression, groupExpression);
        }

        if (finalExpression is null)
            return query;

        var lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
        return query.Where(lambda);
    }

    private static (DateTime lowerBound, DateTime upperBound) GetBounds(FhirDate date)
    {
        // определяем границы дат 
        DateTime lowerBound, upperBound = DateTime.MinValue; //minvalue просто заглушка

        //для ap:
        //Note that the recommended value for the approximation is 10% of the stated value (or for a date, 10% of the gap between now and the date)

        // полная дата, по идее, у нас не может быть дня, но не быть месяца
        if (date.Day.HasValue)
        {
            //задана дата целиком, значит границы одинаковые
            var ecxactDate = new DateTime(date.Year, date.Month!.Value, date.Day.Value);

            // если ap +- 1 день
            lowerBound = date.Prefix == FhirDatePrefix.Ap ? ecxactDate.AddDays(-1) : ecxactDate;
            upperBound = date.Prefix == FhirDatePrefix.Ap ? ecxactDate.AddDays(1) : ecxactDate;
        }
        // только год и месяц
        else if (date.Month.HasValue)
        {
            var lowerDate = new DateTime(date.Year, date.Month.Value, 1); //начало месяца
            var upperDate = lowerDate.AddMonths(1).AddDays(-1); // конец месяца. gt2025-05 - должны быть даты больше 5 месяца
            
            //если ap +- 3 дня
            lowerBound = date.Prefix == FhirDatePrefix.Ap ? lowerDate.AddDays(-3) : lowerDate;
            upperBound = date.Prefix == FhirDatePrefix.Ap ? upperDate.AddDays(3) : upperDate;
        }
        //если только год
        else
        {
            var lowerDate = new DateTime(date.Year, 1, 1); //начало года
            var upperDate = new DateTime(date.Year, 12, 31); // конец года

            //если ap +- 36 дней
            lowerBound = date.Prefix == FhirDatePrefix.Ap ? lowerDate.AddDays(-36) : lowerDate;
            upperBound = date.Prefix == FhirDatePrefix.Ap ? upperDate.AddDays(36) : upperDate;
        }

        return (lowerBound, upperBound);
    }
}
