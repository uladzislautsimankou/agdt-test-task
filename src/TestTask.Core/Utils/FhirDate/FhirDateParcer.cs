namespace TestTask.Core.Utils.FhirDate;

internal static class FhirDateParcer
{
    public static IEnumerable<FhirDate> ParseFhirDate(string rawDate)
    {
        //строка вида date=lt2024-05,ne2024-02-01...
        var qyeryDates = rawDate.Split(',');
        var fhirDates = new List<FhirDate>();

        foreach (var queryDate in qyeryDates)
        {
            var fhirDate = new FhirDate();

            //строка вида date=lt2024-05, ne2024-02-01...
            // если формат date=..., то отсекаем его. остается dateTime с префиксом
            var stringDateTimeWithFrefix = queryDate.Contains('=')
                ? queryDate.Substring(queryDate.IndexOf('=') + 1)
                : queryDate;

            //отсекаем время, если как-то время потом парсить надо, то [1]
            var stringDateWithFrefix = stringDateTimeWithFrefix.Split("T")[0]; 

            var extractedPrefixAndDatePart = ExtractPrefixAndDatePart(stringDateWithFrefix);

            fhirDate.Prefix = extractedPrefixAndDatePart.Prefix;

            // дата в формате 2025-05-19, 2025-05, 2025
            var parts = extractedPrefixAndDatePart.DateParts.Split('-');

            if (parts.Length >= 1 && int.TryParse(parts[0], out int year)) fhirDate.Year = year;
            if (parts.Length >= 2 && int.TryParse(parts[1], out int month)) fhirDate.Month = month;
            if (parts.Length >= 3 && int.TryParse(parts[2], out int day)) fhirDate.Day = day;

            fhirDates.Add(fhirDate);
        }

        return fhirDates;
    }

    private static (FhirDatePrefix Prefix, string DateParts) ExtractPrefixAndDatePart(string input)
    {
        foreach (var prefix in FhirConstants.PrefixMap)
        {
            if (input.StartsWith(prefix.Key, StringComparison.OrdinalIgnoreCase))
            {
                return (prefix.Value, input.Substring(prefix.Key.Length));
            }
        }

        // Без префикса — значит eq
        return (FhirDatePrefix.Eq, input);
    }
}