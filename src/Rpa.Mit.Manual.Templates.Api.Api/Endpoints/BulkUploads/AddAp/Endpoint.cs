using System.Diagnostics.CodeAnalysis;

namespace BulkUploads.AddAp
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddBulkUploadsApEndpoint : Endpoint<BulkUploadsApRequest, Response>
    {


        public AddBulkUploadsApEndpoint(
)
        {

        }

        public override void Configure()
        {
            Post("/bulkuploads/addap");
            AllowFileUploads();
        }

        public override async Task HandleAsync(BulkUploadsApRequest r, CancellationToken ct)
        {
            Response response = new Response();
            await SendAsync(response, 200, cancellation: ct);


        }
    }
}