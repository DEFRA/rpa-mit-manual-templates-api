﻿using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class BulkUploadApHeaderLine : BulkUploadBaseClass
    {
        public string CustomerId { get; set; } = string.Empty;

        public string TotalAmount { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        //public IEnumerable<BulkUploadApDetailLine> BulkUploadDetailLines { get; set; } = Enumerable.Empty<BulkUploadApDetailLine>();
    }
}