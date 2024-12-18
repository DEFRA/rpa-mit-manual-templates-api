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
                fund = dBody switch
                {
                    string x when x == "XG" || x == "INT" => "EXQFund",
                    string x when x == "IP" => "RPAIPFunds",
                    string x when x == "OPA" => "OPAFunds",
                    string x when x == "HE" => "HEFunds",
                    _ => org + "Funds"
                };
            }
            else
            {
                fund = GetFundCodeForNonRPA(dBody, org, invoiceType);
            }

            return fund;
        }

        public string GetDeliveryBodyNamedRange(string org, string dBody, string invoiceType)
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
                case "RDTDom":
                    if (invoiceType == "AR")
                    {
                        deliveryBodyNamedRange = "RDTDomDBs";
                    }
                    break;
            }

            return deliveryBodyNamedRange;
        }

        public string GetAccountNamedRange(string org, string dBody, string invoiceType)
        {
            string accountNamedRange;

            if (org != "NE")
            {
                accountNamedRange = dBody + invoiceType + "Accounts"; // Set the Main Account Named Ranges
            }
            else
            {
                accountNamedRange = GetNonNEAccount(org, dBody, invoiceType);
            }

            if (dBody == "RDTEXQ" && invoiceType == "AP")
            {
                accountNamedRange = "ExNRDPEAPAccounts";
            }
            else if (dBody == "RDTEXQ" && invoiceType == "AR")
            {
                accountNamedRange = "ExNRDPEARAccounts";
            }

            return accountNamedRange;
        }

        public string GetMarketingYearNamedRange(string org, string dBody, string invoiceType)
        {
            // 2014 onwards plus an NA option for Exchequer invoices

            switch (dBody.Right(2))
            {
                case "P1":
                    return "P1MY";
                case "XQ":
                    return "ExNRDPEMY";
            }

            if (dBody.Right(2) == "LS" || dBody == "LSDom") // can't blindly check the substring from pos 5 here like in the VBA
            {
                return "LSMY";
            }
            else if (dBody.Right(2) == "CS" || dBody == "CSDom") // can't blindly check the substring from pos 5 here like in the VBA
            {
                return "NAMY"; // a-VA02-02 request from NE to allow NA for EXQ lines (applies to FC as well)
            }

            string marketingYearNamedRange = dBody switch
            {
                string x when x == "SPS" || x == "TR" => "LSMY",
                string x when x == "RDTDom" && invoiceType == "AR" => "LSMY",
                string x when x == "XG" => "XGMY",
                string x when x == "OPA" => "OPAMY",
                _ => "NSMY"
            };

            return marketingYearNamedRange;
        }

        public string GetSchemeTypeNamedRange(string org, string dBody, string invoiceType)
        {
            string schemeNamedRange;

            if (dBody == "NEP1")
            {
                schemeNamedRange = "P1Schemes";
            }
            else if(dBody == "RDTEXQ")
            {
                schemeNamedRange = "ExNRDPESchemes";
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

        /// <summary>
        /// this to pass SONAR anal complexity checks
        /// </summary>
        /// <param name="dBody"></param>
        /// <param name="org"></param>
        /// <param name="invoiceType"></param>
        /// <returns></returns>
        private static string GetFundCodeForNonRPA(string dBody, string org, string invoiceType)
        {
            string fundCodeNamedRange = "";

            switch (dBody.Right(2))
            {
                case "P1":
                    return "P1Funds";
                case "XQ":
                    return "ExNRDPEFunds";
                case "EA":
                    return dBody.Right(5) == "CSDom" || invoiceType == "AP" ? "EA_DOM_FUNDS" : "EAFunds";
            }

            if (dBody.Right(2) == "LS" && invoiceType == "AR")
            {
                fundCodeNamedRange = (org == "NE" || org == "FC" || org == "RDPE" || org == "RDT") ? "AR_LS_FUNDS" : "LSFunds";
            }

            fundCodeNamedRange = GetFundCodeForNonRPASub(dBody, org, invoiceType, fundCodeNamedRange);
            
            return fundCodeNamedRange;
        }

        /// <summary>
        /// this to pass SONAR anal complexity checks
        /// </summary>
        /// <param name="dBody"></param>
        /// <param name="org"></param>
        /// <param name="invoiceType"></param>
        /// <param name="fundCodeNamedRange"></param>
        /// <returns></returns>
        private static string GetFundCodeForNonRPASub(string dBody, string org, string invoiceType, string fundCodeNamedRange)
        {
            if (dBody.Right(3) == "Dom" && invoiceType == "AR")
            {
                if (dBody == "RDTDom")
                {
                    fundCodeNamedRange = "RDTDomARFunds";
                }
                else if (org == "NE" || org == "FC" || org == "RDPE" || org == "RDT")
                {
                    fundCodeNamedRange = "AR_DOM_FUNDS";
                }
            }
            else
            {
                fundCodeNamedRange = org == "FC" && invoiceType == "AP" ? "FCAP" : FundCodeNamedRangeForOrgAndInvoiceType(dBody, invoiceType, org);
            }

            return fundCodeNamedRange;
        }

        /// <summary>
        /// this to pass SONAR anal complexity checks
        /// </summary>
        /// <param name="dBody"></param>
        /// <param name="invoiceType"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        private static string FundCodeNamedRangeForOrgAndInvoiceType(string dBody, string invoiceType, string org)
        {
            if (org == "RDT" && invoiceType == "AP")
            {
               return "RDTNSFunds";
            }
            else if (dBody == "NECS" && invoiceType == "AP")
            {
                return "NECSFunds";
            }
            else
            {
                return invoiceType == "AR" ? "NSARFunds" : "NSFunds";
            }
        }

        /// <summary>
        /// this to pass SONAR anal complexity checks
        /// </summary>
        /// <param name="org"></param>
        /// <param name="dBody"></param>
        /// <param name="invoiceType"></param>
        /// <returns></returns>
        private static string GetNonNEAccount( string org, string dBody, string invoiceType)
        {
            if (dBody == "NEP1")
            {
                return "NEP1" + invoiceType + "Accounts"; // Set the Main Account Named Ranges
            }
            else if (dBody == "NECS" && invoiceType == "AP")
            {
                return "NECSAPAccounts";
            }
            else if (invoiceType == "AP")
            {
                return org + "APAccounts";
            }
            else
            {
                if (dBody.Right(2) == "LS")
                {
                    return "NELSARAccounts";  // This accommodates the difference between LS AR Accounts and NS AR Accounts
                }
                else if (dBody == "LSDom")
                {
                    return "NELSDomARAccounts";
                }
                else if (dBody == "CSDom")
                {
                    return "NECSDomARAccounts";
                }
                else
                {
                    return "NEARAccounts"; // Not sure if we actually need a different naming convention now
                }
            }
        }
    }
}
