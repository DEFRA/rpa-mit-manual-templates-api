using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Api
{
    [ExcludeFromCodeCoverage]
    public static class StringExtensions
    {
        public static string Right(this string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }
    }
}
