namespace TestRunnerLib
{

    /*****************************************************************************
    
    Support class StringExtensions by Microsoft
    Ref. https://docs.microsoft.com/en-us/dotnet/api/system.string.contains

    enum StringComparison:
    CurrentCulture              Compare strings using culture-sensitive sort rules and the current culture.
    CurrentCultureIgnoreCase    Compare strings using culture-sensitive sort rules, the current culture, and ignoring the case of the strings being compared.
    InvariantCulture            Compare strings using culture-sensitive sort rules and the invariant culture.
    InvariantCultureIgnoreCase	Compare strings using culture-sensitive sort rules, the invariant culture, and ignoring the case of the strings being compared.
    Ordinal                     Compare strings using ordinal (binary) sort rules.
    OrdinalIgnoreCase           Compare strings using ordinal (binary) sort rules and ignoring the case of the strings being compared.

    ******************************************************************************/

    using System;

    public static class StringExtensions
    {
        public static bool Contains(this String str, String substring,
                                    StringComparison comp)
        {
            if (substring == null)
                throw new ArgumentNullException("substring",
                                                "substring cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
                throw new ArgumentException("comp is not a member of StringComparison",
                                            "comp");

            return str.IndexOf(substring, comp) >= 0;
        }
    }
}
