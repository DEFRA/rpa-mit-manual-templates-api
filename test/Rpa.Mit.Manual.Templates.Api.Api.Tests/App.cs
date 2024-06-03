using System.Security.Claims;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests
{
    public class App : WebApplicationFactory<Program>
    {
        private readonly ServiceDescriptor[] _overrides;

        public App(params ServiceDescriptor[]? overrides)
        {
            _overrides = overrides ?? Array.Empty<ServiceDescriptor>();
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                foreach (var service in _overrides)
                {
                    services.Replace(service);
                }
            });

            return base.CreateHost(builder);
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static void WithIdentity(this ClaimsPrincipal user, params Claim[] claims)
            => user.AddIdentity(new ClaimsIdentity(claims, "test_auth"));
    }

    public class TestTimeProvider
    {
        public DateTimeOffset UtcNow { get; set; }
            = DateTimeOffset.UtcNow;

        public void SetTime(int hour, int minutes, int seconds = 0)
        {
            var now = DateTimeOffset.UtcNow.Date;
            UtcNow = new DateTimeOffset(
                now.Year,
                now.Month,
                now.Day,
                hour,
                minutes,
                seconds,
                TimeSpan.Zero
            );
        }
    }
}
