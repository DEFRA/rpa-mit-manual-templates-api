using Rpa.Mit.Manual.Templates.Api.Api.Services;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.NamedRangeTests
{
    /// <summary>
    /// AP tests for the org=EA, dBody=EA combination
    /// </summary>
    public class APNamedRangeTests_Org_EA_DBody_EA : BaseAPTests
    {
        private readonly string _org = "EA";
        private readonly string _dBody = "EA";

        [Fact]
        public void StrOrg_EA_DBody_EA_FundCode()
        {
            var fundNamedRange = _namedRangeService.GetFundCodeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("EA_DOM_FUNDS", fundNamedRange);
        }

        [Fact]
        public void StrOrg_EA_DBody_EA_MainAccount()
        {
            var range = _namedRangeService.GetAccountNamedRange(_org, _dBody, _accountType);

            Assert.Equal("EAAPAccounts", range);
        }

        [Fact]
        public void StrOrg_EA_DBody_EA_Scheme()
        {
            var range = _namedRangeService.GetSchemeTypeNamedRange(_org, _dBody, _accountType);

            Assert.Equal("EASchemes", range);
        }

        [Fact]
        public void StrOrg_EA_DBody_EA_MarketingYear()
        {
            var range = _namedRangeService.GetMarketingYearNamedRange(_org, _dBody);

            Assert.Equal("NSMY", range);
        }

        [Fact]
        public void StrOrg_EA_DBody_EA_DeliveryBody()
        {
            var range = _namedRangeService.GetDeliveryBodyNamedRange(_org, _dBody);

            Assert.Equal("EADBs", range);
        }
    }
}
