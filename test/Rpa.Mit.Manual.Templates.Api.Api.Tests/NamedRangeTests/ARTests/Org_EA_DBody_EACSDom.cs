namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests.ARTests
{
    public class Org_EA_DBody_EACSDom : BaseARTests
    {
        private readonly string _org = "EA";
        private readonly string _dBody = "EACSDom";

        [Fact]
        public void FundCode()
        {
            var fundNamedRange = _namedRangeService.GetFundCodeNamedRange(_org, _dBody, AccountType);

            Assert.Equal("EA_DOM_FUNDS", fundNamedRange);
        }

        [Fact]
        public void MainAccount()
        {
            var range = _namedRangeService.GetAccountNamedRange(_org, _dBody, AccountType);

            Assert.Equal("EACSDomARAccounts", range);
        }

        [Fact]
        public void Scheme()
        {
            var range = _namedRangeService.GetSchemeTypeNamedRange(_org, _dBody, AccountType);

            Assert.Equal("EACSDomSchemes", range);
        }

        [Fact]
        public void MarketingYear()
        {
            var range = _namedRangeService.GetMarketingYearNamedRange(_org, _dBody, AccountType);

            Assert.Equal("NAMY", range);
        }

        [Fact]
        public void DeliveryBody()
        {
            var range = _namedRangeService.GetDeliveryBodyNamedRange(_org, _dBody, AccountType);

            Assert.Equal("EADBs", range);
        }
    }
}
