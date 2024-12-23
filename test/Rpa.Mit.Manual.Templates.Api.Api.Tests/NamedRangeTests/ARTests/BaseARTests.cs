using Rpa.Mit.Manual.Templates.Api.Api.Services;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests.ARTests
{
    public abstract class BaseARTests
    {
        public readonly string AccountType = "AR";
        public readonly NamedRangeService _namedRangeService = new NamedRangeService();
    }
}
