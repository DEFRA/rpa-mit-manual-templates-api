namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests.ARTests
{
    public class Org_EA_DBody_EA : BaseARTests
    {
        private readonly string _org = "EA";
        private readonly string _dBody = "EA";

        [Fact]
        public void FundCode()
        {
            var fundNamedRange = _namedRangeService.GetFundCodeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("EAFunds", fundNamedRange);
        }

        [Fact]
        public void MainAccount()
        {
            var range = _namedRangeService.GetAccountNamedRange(_org, _dBody, _accountType);

            Assert.Equal("EAARAccounts", range);
        }

        [Fact]
        public void Scheme()
        {
            var range = _namedRangeService.GetSchemeTypeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("EASchemes", range);
        }

        [Fact]
        public void MarketingYear()
        {
            var range = _namedRangeService.GetMarketingYearNamedRange(_org, _dBody, _accountType);

            Assert.Equal("NSMY", range);
        }

        [Fact]
        public void DeliveryBody()
        {
            var range = _namedRangeService.GetDeliveryBodyNamedRange(_org, _dBody, _accountType);

            Assert.Equal("EADBs", range);
        }
    }
}
