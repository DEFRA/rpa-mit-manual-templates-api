using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Admin;

namespace Approvers
{
    [ExcludeFromCodeCoverage]
    internal sealed class Response
    {
        public IEnumerable<AdminApprover> AdminApprovers { get; set; } = Enumerable.Empty<AdminApprover>();

        public string Message { get; set; } = string.Empty;
    }
}
