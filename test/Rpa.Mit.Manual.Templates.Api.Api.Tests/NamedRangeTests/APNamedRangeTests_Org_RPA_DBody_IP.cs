namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests
{
    /// <summary>
    /// AP tests for the org=RPA, dBody=IP combination
    /// </summary>
    public class APNamedRangeTests_Org_RPA_DBody_IP : BaseAPTests
    {
        private readonly string _org = "RPA";
        private readonly string _dBody = "IP";

        [Fact]
        public void FundCode()
        {
            var namedRange = _namedRangeService.GetFundCodeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("RPAIPFunds", namedRange);
        }

        [Fact]
        public void MainAccount()
        {
            var namedRange = _namedRangeService.GetAccountNamedRange(_org, _dBody, _accountType);

            Assert.Equal("IPAPAccounts", namedRange);
        }

        [Fact]
        public void Scheme()
        {
            var namedRange = _namedRangeService.GetSchemeTypeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("IPSchemes", namedRange);
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
