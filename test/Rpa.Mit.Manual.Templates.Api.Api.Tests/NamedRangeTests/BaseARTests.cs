using Rpa.Mit.Manual.Templates.Api.Api.Services;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests
{
    public abstract class BaseARTests
    {
        public readonly string _accountType = "AR";
        public readonly NamedRangeService _namedRangeService = new NamedRangeService();
    }
}
