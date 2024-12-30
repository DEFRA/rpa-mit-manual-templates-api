namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests.ARTests
{
    public class Org_RPA_DBody_OPA : BaseARTests
    {
        private readonly string _org = "RPA";
        private readonly string _dBody = "OPA";

        [Fact]
        public void FundCode()
        {
            var fundNamedRange = _namedRangeService.GetFundCodeNamedRange(_org, _dBody, AccountType);

            Assert.Equal("OPAFunds", fundNamedRange);
        }

        [Fact]
        public void MainAccount()
        {
            var range = _namedRangeService.GetAccountNamedRange(_org, _dBody, AccountType);

            Assert.Equal("OPAARAccounts", range);
        }

        [Fact]
        public void Scheme()
        {
            var range = _namedRangeService.GetSchemeTypeNamedRange(_org, _dBody, AccountType);

            Assert.Equal("OPASchemes", range);
        }

        [Fact]
        public void MarketingYear()
        {
            var range = _namedRangeService.GetMarketingYearNamedRange(_org, _dBody, AccountType);

            Assert.Equal("OPAMY", range);
        }

        [Fact]
        public void DeliveryBody()
        {
            var range = _namedRangeService.GetDeliveryBodyNamedRange(_org, _dBody, AccountType);

            Assert.Equal("OPADBs", range);
        }
    }
}
