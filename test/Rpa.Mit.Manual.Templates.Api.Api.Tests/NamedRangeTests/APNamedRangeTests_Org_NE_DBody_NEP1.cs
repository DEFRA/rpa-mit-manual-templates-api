namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests
{
    public class APNamedRangeTests_Org_NE_DBody_NEP1 : BaseAPTests
    {
        private readonly string _org = "NE";
        private readonly string _dBody = "NEP1";

        [Fact]
        public void FundCode()
        {
            var namedRange = _namedRangeService.GetFundCodeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("P1Funds", namedRange);
        }

        [Fact]
        public void MainAccount()
        {
            var namedRange = _namedRangeService.GetAccountNamedRange(_org, _dBody, _accountType);

            Assert.Equal("NEP1APAccounts", namedRange);
        }

        [Fact]
        public void Scheme()
        {
            var namedRange = _namedRangeService.GetSchemeTypeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("P1Schemes", namedRange);
        }

        [Fact]
        public void MarketingYear()
        {
            var namedRange = _namedRangeService.GetMarketingYearNamedRange(_org, _dBody, _accountType);

            Assert.Equal("P1MY", namedRange);
        }

        [Fact]
        public void DeliveryBody()
        {
            var namedRange = _namedRangeService.GetDeliveryBodyNamedRange(_org, _dBody, _accountType);

            Assert.Equal("P1DBs", namedRange);
        }
    }
}
