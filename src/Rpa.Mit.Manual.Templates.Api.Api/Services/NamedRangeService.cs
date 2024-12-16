using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Services
{
    [ExcludeFromCodeCoverage]
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
            string accountNamedRange = string.Empty;

            if (org != "NE")
            {
                accountNamedRange = dBody + invoiceType + "Accounts"; // Set the Main Account Named Ranges
            }
            else
            {
                if (dBody == "NEP1")
                {
                    accountNamedRange = "NEP1" + invoiceType + "Accounts"; // Set the Main Account Named Ranges
                }
                else if (dBody == "NECS" && invoiceType == "AP")
                {
                    accountNamedRange = "NECSAPAccounts";
                }
                else if (invoiceType == "AP")
                {
                    accountNamedRange = org + "APAccounts";
                }
                else
                {
                    if (dBody.Right(2) == "LS")
                    {
                        accountNamedRange = "NELSARAccounts";  // This accommodates the difference between LS AR Accounts and NS AR Accounts
                    }
                    else if (dBody == "LSDom")
                    {
                        accountNamedRange = "NELSDomARAccounts";
                    }
                    else if (dBody == "CSDom")
                    {
                        accountNamedRange = "NECSDomARAccounts";
                    }
                    else
                    {
                        accountNamedRange = "NEARAccounts"; // Not sure if we actually need a different naming convention now
                    }
                }
            }

            return accountNamedRange;
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

            if (dBody.Right(2) == "LS" || dBody == "LSDom") // can't blindly check the substring from pos 5 here like in the VBA
            {
                marketingYearNamedRange = "LSMY";
            }
            else if (dBody.Right(2) == "CS" || dBody == "CSDom") // can't blindly check the substring from pos 5 here like in the VBA
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

        public string GetSchemeTypeNamedRange(string org, string dBody, string invoiceType)
        {
            string schemeNamedRange;

            if (dBody == "NEP1")
            {
                schemeNamedRange = "P1Schemes";
            }
            else if (org == "NE" && dBody.Right(2) == "LS")
            {
                if (invoiceType == "AR")
                {
                    schemeNamedRange = "NEARSchemes";
                }
                else
                {
                    schemeNamedRange = "NEAPSchemes";  // Excludes the AR Only Schemes for NE
                }
            }
            else
            {
                schemeNamedRange = dBody + "Schemes";  // Simpler if you are not NE
            }

            return schemeNamedRange;
        }

        private static string GetFundCodeForNonRPA(string dBody, string org, string invoiceType)
        {
            string fundCodeNamedRange = "";

            switch (dBody.Right(2))
            {
                case "P1":
                    fundCodeNamedRange = "P1Funds";
                    break;
                case "XQ":
                    fundCodeNamedRange = "ExNRDPEFunds";
                    break;
                case "EA":
                    fundCodeNamedRange = dBody.Right(5) == "CSDom" || invoiceType == "AP" ? "EA_DOM_FUNDS" : "EAFunds";
                    break;
                case "LS":
                    fundCodeNamedRange = FundCodeNamedRangeForLs(invoiceType, org);
                    break;
            }
            
            if (dBody.Right(3) == "Dom" && invoiceType == "AR")
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
                else
                {
                    fundCodeNamedRange = invoiceType == "AR" ? "NSARFunds" : "NSFunds";
                }
            }

            return fundCodeNamedRange;
        }

        private static string FundCodeNamedRangeForLs(string invoiceType, string org) => 
            invoiceType == "AR" && (org == "NE" || org == "FC" || org == "RDPE" || org == "RDT") ? "AR_LS_FUNDS" : "LSFunds";
    }
}
