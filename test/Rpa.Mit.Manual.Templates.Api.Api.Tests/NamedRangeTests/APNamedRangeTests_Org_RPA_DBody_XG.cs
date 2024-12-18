namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests
{
    public class APNamedRangeTests_Org_RPA_DBody_XG : BaseAPTests
    {
        private readonly string _org = "RPA";
        private readonly string _dBody = "XG";

        [Fact]
        public void FundCode()
        {
            var namedRange = _namedRangeService.GetFundCodeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("EXQFund", namedRange);
        }

        [Fact]
        public void MainAccount()
        {
            var namedRange = _namedRangeService.GetAccountNamedRange(_org, _dBody, _accountType);

            Assert.Equal("XGAPAccounts", namedRange);
        }

        [Fact]
        public void Scheme()
        {
            var namedRange = _namedRangeService.GetSchemeTypeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("XGSchemes", namedRange);
        }

        [Fact]
        public void MarketingYear()
        {
            var namedRange = _namedRangeService.GetMarketingYearNamedRange(_org, _dBody, _accountType);

            Assert.Equal("XGMY", namedRange);
        }

        [Fact]
        public void DeliveryBody()
        {
            var namedRange = _namedRangeService.GetDeliveryBodyNamedRange(_org, _dBody, _accountType);

            Assert.Equal("RPADBs", namedRange);
        }
    }
}
