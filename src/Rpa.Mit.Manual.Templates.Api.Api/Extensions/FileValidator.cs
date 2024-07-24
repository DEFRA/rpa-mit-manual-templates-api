using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class FileValidator
    {
        public static bool IsFileExtensionAllowed(this IFormFile file, string[] allowedExtensions)
        {
            var extension = Path.GetExtension(file.FileName);
            return allowedExtensions.Contains(extension);
        }

        public static bool IsFileSizeWithinLimit(this IFormFile file, double maxSizeInBytes)
        {
            return file.Length <= maxSizeInBytes;
        }
    }
}
