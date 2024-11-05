using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Admin;

namespace AdminAdd
{
    [ExcludeFromCodeCoverage]
    internal sealed class Request
    {

        public required AdminApprover AdminApprover { get; set; }

        internal sealed class Validator : Validator<Request>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class Response
    {
        public bool Result { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
