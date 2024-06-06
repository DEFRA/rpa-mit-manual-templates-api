using Xunit.Abstractions;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.EndpointTests
{
    public class Root
    {
        private readonly ITestOutputHelper _output;
        private TestTimeProvider Clock { get; } = new();
        private App App { get; }

        public Root(ITestOutputHelper output)
        {
            _output = output;
            App = new();
        }

        [Fact]
        public async Task CanGetRootEndpointWithWrongTime()
        {
            // Arrange
            Clock.SetTime(0, 0);
            // Act
            var client = App.CreateClient();

            var result = await client.GetStringAsync("/");

            // Assert
            Assert.NotEqual("Hello, World @ 12:00:00 AM", result);

            _output.WriteLine(result);
        }
    }
}
