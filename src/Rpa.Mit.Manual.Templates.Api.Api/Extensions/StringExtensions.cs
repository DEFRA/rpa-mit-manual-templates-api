using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Api
{
    [ExcludeFromCodeCoverage]
    public static class StringExtensions
    {
        public static string Left(this string str, int length)
        {
            return str.Substring(0, length);
        }

        public static string Right(this string str, int length)
        {
            return str.Length < length ? str : str.Substring(str.Length - length, length);
        }
    }
}
