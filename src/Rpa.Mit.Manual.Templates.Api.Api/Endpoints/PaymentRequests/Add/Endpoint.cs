﻿namespace PaymentsRequests.Add
{
    internal sealed class Endpoint : Endpoint<Request, Response, Mapper>
    {
        public override void Configure()
        {
            Post("/paymentrequest/add");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            await SendAsync(new Response());
        }
    }
}