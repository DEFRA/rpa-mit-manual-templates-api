namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests
{
    public class APNamedRangeTests_Org_RDT_DBody_RDTCP : BaseAPTests
    {
        private readonly string _org = "RDT";
        private readonly string _dBody = "RDTCP";

        [Fact]
        public void FundCode()
        {
            var namedRange = _namedRangeService.GetFundCodeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("RDTNSFunds", namedRange);
        }

        [Fact]
        public void MainAccount()
        {
            var namedRange = _namedRangeService.GetAccountNamedRange(_org, _dBody, _accountType);

            Assert.Equal("RDTCPAPAccounts", namedRange);
        }

        [Fact]
        public void Scheme()
        {
            var namedRange = _namedRangeService.GetSchemeTypeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("RDTCPSchemes", namedRange);
        }

        [Fact]
        public void MarketingYear()
        {
            var namedRange = _namedRangeService.GetMarketingYearNamedRange(_org, _dBody);

            Assert.Equal("NSMY", namedRange);
        }

        [Fact]
        public void DeliveryBody()
        {
            var namedRange = _namedRangeService.GetDeliveryBodyNamedRange(_org, _dBody);

            Assert.Equal("RDTDBs", namedRange);
        }
    }
}
