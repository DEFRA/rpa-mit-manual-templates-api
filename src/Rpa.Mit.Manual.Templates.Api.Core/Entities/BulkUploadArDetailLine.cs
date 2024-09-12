using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class BulkUploadArDetailLine : BulkUploadApDetailLine
    {
        /// <summary>
        /// calculated. either ‘ADMIN ERROR’ or ‘IRREGULARITY’
        /// </summary>
        public string DebtType { get; set; } = string.Empty;
    }
}
