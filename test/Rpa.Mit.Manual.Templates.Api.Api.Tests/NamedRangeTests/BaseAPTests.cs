using Rpa.Mit.Manual.Templates.Api.Api.Services;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests
{
    public abstract class BaseAPTests
    {
        public readonly string _accountType = "AP";
        public readonly NamedRangeService _namedRangeService = new NamedRangeService();
    }
}
