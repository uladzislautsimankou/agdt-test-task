using TestTask.Core.Utils.FhirDate;

namespace TestTask.Core.Utils;

internal static class FhirConstants
{
    public static readonly Dictionary<string, FhirDatePrefix> PrefixMap = new()
    {
        ["eq"] = FhirDatePrefix.Eq,
        ["ne"] = FhirDatePrefix.Ne,
        ["gt"] = FhirDatePrefix.Gt,
        ["lt"] = FhirDatePrefix.Lt,
        ["ge"] = FhirDatePrefix.Ge,
        ["le"] = FhirDatePrefix.Le,
        ["sa"] = FhirDatePrefix.Sa,
        ["eb"] = FhirDatePrefix.Eb,
        ["ap"] = FhirDatePrefix.Ap,
    };
}
