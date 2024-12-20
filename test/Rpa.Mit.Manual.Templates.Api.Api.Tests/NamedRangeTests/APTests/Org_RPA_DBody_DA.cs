namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests.APTests
{
    /// <summary>
    /// AP tests for the org=RPA, dBody=DA combination
    /// </summary>
    public class Org_RPA_DBody_DA : BaseAPTests
    {
        private readonly string _org = "RPA";
        private readonly string _dBody = "DA";

        [Fact]
        public void FundCode()
        {
            var namedRange = _namedRangeService.GetFundCodeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("RPAFunds", namedRange);
        }

        [Fact]
        public void MainAccount()
        {
            var namedRange = _namedRangeService.GetAccountNamedRange(_org, _dBody, _accountType);

            Assert.Equal("DAAPAccounts", namedRange);
        }

        [Fact]
        public void Scheme()
        {
            var namedRange = _namedRangeService.GetSchemeTypeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("DASchemes", namedRange);
        }

        [Fact]
        public void MarketingYear()
        {
            var namedRange = _namedRangeService.GetMarketingYearNamedRange(_org, _dBody, _accountType);

            Assert.Equal("NSMY", namedRange);
        }

        [Fact]
        public void DeliveryBody()
        {
            var namedRange = _namedRangeService.GetDeliveryBodyNamedRange(_org, _dBody, _accountType);

            Assert.Equal("RPADBs", namedRange);
        }
    }
}
