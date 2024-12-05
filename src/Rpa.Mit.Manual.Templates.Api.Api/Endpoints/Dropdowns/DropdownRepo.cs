using System.Data;

using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

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
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                return await cn.QueryAsync<Dropdown>("SELECT name, values FROM lookup_dropdowns");
            }
        }
    }
}
