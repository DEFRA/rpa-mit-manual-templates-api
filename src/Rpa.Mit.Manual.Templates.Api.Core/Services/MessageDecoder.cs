using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Rpa.Mit.Manual.Templates.Api.Core.Services
{
    [ExcludeFromCodeCoverage]
    public static class MessageDecoder
    {
        public static string DecodeMessage(this string message)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(message));
        }

        public static string EncodeMessage(this string message)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        }
    }
}
