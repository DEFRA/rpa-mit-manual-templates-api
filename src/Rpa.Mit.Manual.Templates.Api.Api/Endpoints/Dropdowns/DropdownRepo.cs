using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Dropdowns
{
    public class DropdownRepo : BaseData, IDropdownRepo
    {
        public DropdownRepo(IOptions<PostGres> options) : base(options)
        { }

        public async Task<IEnumerable<Dropdown>> GetAll(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
