namespace TestTask.Core.Utils.FhirDate;

internal enum FhirDatePrefix
{
    Eq, // ==
    Ne, // !=
    Gt, //>
    Lt, //<
    Ge, //>=
    Le, //<=
    Sa, //starts after только для полей типа period
    Eb, //ends before только для полей типа period
    Ap  //approximately
}
