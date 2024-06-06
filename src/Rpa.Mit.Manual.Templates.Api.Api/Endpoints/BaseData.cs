using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseData
    {
        private readonly IOptions<ConnectionStrings> _options;

        protected BaseData(IOptions<ConnectionStrings> options) 
        {
            _options = options;
        }

        public virtual string DbConn => _options.Value.PostGres;
    }
}
