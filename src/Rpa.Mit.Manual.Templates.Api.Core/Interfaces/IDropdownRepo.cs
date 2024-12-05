using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IDropdownRepo
    {
        Task<IEnumerable<Dropdown>> GetAll(CancellationToken ct);
    }
}
