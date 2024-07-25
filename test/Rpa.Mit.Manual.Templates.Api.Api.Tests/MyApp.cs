using FastEndpoints.Testing;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests
{
    public class MyApp : AppFixture<Program.Program>
    {
        public int Id { get; set; } //some state

        protected override async Task SetupAsync()
        {
            Id = 123; //some setup logic
            await Task.CompletedTask;
        }

        protected override void ConfigureApp(IWebHostBuilder a)
        {
            // do host builder config here
        }

        protected override void ConfigureServices(IServiceCollection s)
        {
            // do test service registration here
        }

        protected override async Task TearDownAsync()
        {
            await Task.CompletedTask;
        }
    }
}
