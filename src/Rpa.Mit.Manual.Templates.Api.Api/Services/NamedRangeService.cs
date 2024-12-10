using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Services
{
    public class NamedRangeService : INamedRangeService
    {
        public string GetFundCodeNamedRange(string org, string dBody, string invoiceType)
        {
            string fund;

            if (org == "RPA")
            {
                switch (dBody)
                {
                    case "XG,INT":
                        fund = "EXQFund";
                        break;
                    case "IP":
                        fund = "RPAIPFunds";
                        break;
                    case "OPA":
                        fund = "OPAFunds";
                        break;
                    case "HE":
                        fund = "HEFunds";
                        break;
                    default:
                        fund = org + "Funds";
                        break;
                }
            }
            else
            {
                fund = GetFundCodeForNonRPA(dBody, org, invoiceType);
            }

            return fund;
        }

        public string GetDeliveryBodyNamedRange(string org, string dBody)
        {
            string deliveryBodyNamedRange;

            switch (dBody.Right(2))
            {
                case "P1":
                    deliveryBodyNamedRange = "P1DBs";
                    break;
                case "XQ":
                    deliveryBodyNamedRange = "ExNRDPEDBs";
                    break;
                default:
                    deliveryBodyNamedRange = org + "DBs";
                    break;
            }

            switch (dBody)
            {
                case "OPA":
                    deliveryBodyNamedRange = "OPADBs";
                    break;
                case "TR":
                    deliveryBodyNamedRange = "TRDBs";
                    break;
            }

            return deliveryBodyNamedRange;
        }

        public string GetAccountNamedRange(string org, string dBody, string invoiceType)
        {
            throw new NotImplementedException();
        }

        public string GetMarketingYearNamedRange(string org, string dBody)
        {
            string marketingYearNamedRange = "NSMY"; // 2014 onwards plus an NA option for Exchequer invoices

            switch (dBody.Right(2))
            {
                case "P1":
                    marketingYearNamedRange = "P1MY";
                    break;
                case "XQ":
                    marketingYearNamedRange = "ExNRDPEMY";
                    break;
            }

            if (dBody.Right(2) == "LS" || dBody.Right(5) == "LSDom")
            {
                marketingYearNamedRange = "LSMY";
            }
            else if (dBody.Right(2) == "CS" || dBody.Right(5) == "CSDom")
            {
                marketingYearNamedRange = "NAMY"; // a-VA02-02 request from NE to allow NA for EXQ lines (applies to FC as well)
            }

            switch (dBody)
            {
                case "SPS,TR":
                    marketingYearNamedRange = "LSMY";
                    break;
                case "XG":
                    marketingYearNamedRange = "XGMY";
                    break;
                case "OPA":
                    marketingYearNamedRange = "OPAMY";
                    break;
            }

            return marketingYearNamedRange;
        }

        public string GetSchemeTypeNamedRange(string org, string dBody)
        {
            throw new NotImplementedException();
        }

        private string GetFundCodeForNonRPA(string dBody, string org, string invoiceType)
        {
            string fundCodeNamedRange = "";

            if (dBody.Right(2) == "P1")
            {
                // What funds for NE, FC, RDT, RDPE schemes - LS or NS
                fundCodeNamedRange = "P1Funds";
            }
            else if (dBody.Right(2) == "XQ")
            {
                fundCodeNamedRange = "ExNRDPEFunds";
            }
            else if (dBody.Right(2) == "LS" && invoiceType == "AR")
            {
                if (org == "NE" || org == "FC" || org == "RDPE" || org == "RDT")
                {
                    fundCodeNamedRange = "AR_LS_FUNDS";
                }
                else
                {
                    fundCodeNamedRange = "LSFunds";
                }
            }
            else if (dBody.Right(3) == "Dom" && invoiceType == "AR")
            {
                if (org == "NE" || org == "FC" || org == "RDPE" || org == "RDT")
                {
                    fundCodeNamedRange = "AR_DOM_FUNDS";
                }
            }
            else if (org == "FC" && invoiceType == "AP") //    'v02-05 added to provide EXQ99 for FC AP (pv)
            {
                fundCodeNamedRange = "FCAP";
            }
            else if (dBody.Right(2) == "EA")            // a-VA03-00 change to accomodate EA Invoices
            {
                if (dBody.Right(5) == "CSDom" || invoiceType == "AP")
                {
                    fundCodeNamedRange = "EA_DOM_FUNDS";
                }
                else
                {
                    fundCodeNamedRange = "EAFunds";
                }
            }
            else
            {
                if (org == "RDT" && invoiceType == "AP")
                {
                    fundCodeNamedRange = "RDTNSFunds";
                }
                else if (dBody == "NECS" && invoiceType == "AP")
                {
                    fundCodeNamedRange = "NECSFunds";
                }
                else if (invoiceType == "AR")
                {
                    fundCodeNamedRange = "NSARFunds";
                }
                else
                {
                    fundCodeNamedRange = "NSFunds";
                }
            }

            return fundCodeNamedRange;
        }
    }
}
